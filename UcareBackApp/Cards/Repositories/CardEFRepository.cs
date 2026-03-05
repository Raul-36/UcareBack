namespace UcareBackApp.Repositories;

using Microsoft.EntityFrameworkCore;

using Npgsql;
using UcareBackApp.Data;
using UcareBackApp.Cards.Entities;
using UcareBackApp.Cards.Repositories.Base;


public class CardEfRepository : ICardRepository
{
    readonly UcareDbContext context;
    public CardEfRepository(UcareDbContext context)
    {
        this.context = context;
    }

    
        public async Task<IEnumerable<Card>> GetCardsAsync()
        {
            return await context.Cards.AsNoTracking().ToListAsync();
        }

        public async Task<Card?> GetCardAsync(Guid id)
        {
            return await context.Cards.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<Card> AddCardAsync(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            var addedCard = (await context.Cards.AddAsync(card)).Entity;
            await context.SaveChangesAsync();
            return addedCard;
        }

        public async Task<Card> UpdateCardAsync(Card updatedCard)
        {
            if (updatedCard == null)
            {
                throw new ArgumentNullException(nameof(updatedCard));
            }
            var findCard = await context.Cards.FindAsync(updatedCard.Id);
            if (findCard == null)
            {
                throw new ArgumentException("Card not found", nameof(updatedCard.Id));
            }

            var updatedCardEntity = context.Cards.Update(updatedCard).Entity;
            await context.SaveChangesAsync();
            return updatedCardEntity;
        }
        public async Task DeleteCardAsync(Guid id)
        {
            var card = await context.Cards.FindAsync(id);
            if (card == null)
            {
                throw new ArgumentException("Card not found", nameof(id));
            }

            context.Cards.Remove(card);
            await context.SaveChangesAsync();
        }

}

