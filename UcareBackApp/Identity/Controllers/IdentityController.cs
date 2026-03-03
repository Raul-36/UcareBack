using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UcareBackApp.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using UcareBackApp.Identity.Entities;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly UserManager<UcareUser> _userManager;
    private readonly RoleManager<UcareRole> _roleManager;
    private readonly SignInManager<UcareUser> _signInManager;
    private readonly string _jwtSecret;

    public IdentityController(
        UserManager<UcareUser> userManager,
        RoleManager<UcareRole> roleManager,
        SignInManager<UcareUser> signInManager,
        IConfiguration config)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _jwtSecret = config["JwtSettings:Secret"]
            ?? throw new ArgumentNullException("JwtSettings:Secret not configured");
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            return BadRequest(new[] { "This email is already in use" });
        }

        var newUser = new UcareUser
        {
            Email = dto.Email,
            UserName = dto.Name
        };

        var result = await _userManager.CreateAsync(newUser, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors.Select(e => e.Description));
        }

        if (!await _roleManager.RoleExistsAsync("user"))
        {
            await _roleManager.CreateAsync(new UcareRole
            {
                Name = "user"
            });
        }

        await _userManager.AddToRoleAsync(newUser, "user");

        await _signInManager.SignInAsync(newUser, isPersistent: false);

        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return BadRequest(new[] { "Incorrect email or password" });
        }

        var token = await GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return BadRequest(new[] { "Incorrect email or password" });
        }

        var signInResult = await _signInManager.PasswordSignInAsync(
            user,
            dto.Password,
            isPersistent: false,
            lockoutOnFailure: false);

        if (!signInResult.Succeeded)
        {
            return BadRequest(new[] { "Incorrect email or password" });
        }

        var token = await GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    private async Task<string> GenerateJwtToken(UcareUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName ?? ""),
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "UcareApp",
            audience: "UcareApp",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}