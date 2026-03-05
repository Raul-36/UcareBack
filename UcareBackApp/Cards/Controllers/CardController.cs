using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http; 
using Microsoft.AspNetCore.Mvc;
using UcareBackApp.Cards.Dtos.Requests;
using UcareBackApp.Cards.Dtos.Responses;
using UcareBackApp.Cards.Services.Base;

[ApiController]
[Route("api/[controller]")]
public class CardsController : ControllerBase
{
    private readonly ICardService cardService;
    public CardsController(ICardService cardService)
    {
        this.cardService = cardService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ShortCardResponse>>> GetCards()
    {
        return Ok(await cardService.GetCardsAsync());  
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FullCardResponse>> GetCard(Guid id)
    {
        try
        {
            var card = await cardService.GetCardAsync(id);
            return Ok(card);
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<FullCardResponse>> PostCard([FromBody] CreateCardRequest request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Invalid user identifier.");
        }

        try
        {
            var createdCard = await cardService.AddCardAsync(request, userId);
            return CreatedAtAction(nameof(GetCard), new { id = createdCard.Id }, createdCard);
        }
        catch (BadHttpRequestException e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpPut()]
    public async Task<IActionResult> PutCard([FromBody] UpdateCardRequest request)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Invalid user identifier.");
        }

        try
        {
            await cardService.UpdateCardInfoAsync(request, userId);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return StatusCode(StatusCodes.Status403Forbidden, e.Message);
        }
        
    }

    [Authorize]
    [HttpPut("{id}/image")]
    public async Task<IActionResult> UpdateCardImage(Guid id, IFormFile image)
    {
        try
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!Guid.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Invalid user identifier.");
            }
            await cardService.UpdateCardImageAsync(id, image, userId);
            return NoContent();
        }
        catch (KeyNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return StatusCode(StatusCodes.Status403Forbidden, e.Message);
        }
    }


    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCard(Guid id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized("Invalid user identifier.");
        }

        try
        {
            await cardService.DeleteCardAsync(id, userId);
            return NoContent();
        }
        catch (UnauthorizedAccessException e)
        {
            return StatusCode(StatusCodes.Status403Forbidden, e.Message);
        }
    }
}

