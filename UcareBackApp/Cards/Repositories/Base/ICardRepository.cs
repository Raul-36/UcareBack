using UcareBackApp.Cards.Entities;

namespace UcareBackApp.Cards.Repositories.Base;
public interface ICardRepository
{
    public Task<IEnumerable<Card>> GetCardsAsync();

    public Task<Card?> GetCardAsync(Guid id);

    public Task<Card> AddCardAsync(Card card);

    public Task<Card> UpdateCardAsync(Card updatedCard);

    public Task DeleteCardAsync(Guid id);
}