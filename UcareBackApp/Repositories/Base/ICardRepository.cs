using UcareBackApp.Models;

namespace UcareBackApp.Repositories.Base;
public interface ICardRepository
{
    public Task<IEnumerable<Card>> GetCardsAsync();

    public Task<Card> GetCardAsync(int id);

    public Task AddCardAsync(Card card);

    public Task UpdateCardAsync(Card updatedCard);

    public Task DeleteCardAsync(int id);
}