using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using UcareBackApp.Models;
using UcareBackApp.Repositories.Base;

[ApiController]
[Route("api/[controller]")]
public class CardController : ControllerBase
{
    readonly ICardRepository cards;
    public CardController(ICardRepository cards)
    {
        this.cards = cards;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Card>>> GetCards()
    {
        return Ok(await cards.GetCardsAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Card>> GetCard(int id)
    {
        var card = await cards.GetCardAsync(id);
        if (card == null)
        {
            return NotFound();
        }
        return Ok(card);
    }

    [HttpPost]
    public async Task<ActionResult<Card>> PostCard(Card card)
    {
        var email = User.FindFirstValue(ClaimTypes.Email);
        card.UserEmail = email;
        await cards.AddCardAsync(card);
        return CreatedAtAction(nameof(GetCard), new { id = card.Id }, card);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutCard(int id, Card card)
    {
        if (id != card.Id)
        {
            return BadRequest();
        }

        var existingCard = await cards.GetCardAsync(id);
        if (existingCard == null)
        {
            return NotFound();
        }

        await cards.UpdateCardAsync(card);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCard(int id)
    {
        var card = await cards.GetCardAsync(id);
        if (card == null)
        {
            return NotFound();
        }

        await cards.DeleteCardAsync(id);
        return NoContent();
    }
}
