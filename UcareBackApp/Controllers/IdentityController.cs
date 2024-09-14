using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UcareBackApp.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text.Json;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly SignInManager<IdentityUser> signInManager;
    private readonly string _jwtSecret;

    public IdentityController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<IdentityUser> signInManager, IConfiguration config)
    {
        this.roleManager = roleManager;
        this.userManager = userManager;
        this.signInManager = signInManager;
        _jwtSecret = config["JwtSettings:Secret"];
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var existingUser = await userManager.FindByEmailAsync(dto.Email);
        if (existingUser != null)
        {
             return BadRequest(JsonSerializer.Serialize<IEnumerable<string>>(["This email is already in use"]));
        }
        var newUser = new IdentityUser()
        {
            Email = dto.Email,
            UserName = dto.Name,
        };

        var result = await userManager.CreateAsync(newUser, dto.Password);

        if (result.Succeeded == false)
        {
            return base.BadRequest(result.Errors.Select(e => e.Description));
        }
        await roleManager.CreateAsync(new IdentityRole()
        {
            Name = "user",
        });
        await userManager.AddToRoleAsync(newUser, "user");

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = this.userManager.Users.FirstOrDefault(u => u.Email == dto.Email);
        if (user == null)
        {
            return BadRequest(JsonSerializer.Serialize<IEnumerable<string>>(["Incorrect email or password"]));
        }
        var signInResult = await this.signInManager.PasswordSignInAsync(user, dto.Password, true, true);
        if (signInResult.Succeeded == false)
        {
            return BadRequest(JsonSerializer.Serialize<IEnumerable<string>>(["Incorrect email or password"]));
        }


        var token = GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        string role = "user";
        if (user.Email == "admin@gmail.com")
        {
            role = "admin";

        }
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSecret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "yourapp",
            audience: "yourapp",
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}