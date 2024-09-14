namespace UcareBackApp.Repositories;

using Microsoft.EntityFrameworkCore;

using Npgsql;
using UcareBackApp.Data;
using UcareBackApp.Models;
using UcareBackApp.Repositories.Base;


public class CardEfRepository : ICardRepository
{
    readonly UcareDbContext context;
    public CardEfRepository(UcareDbContext context)
    {
        this.context = context;
    }

    
        public async Task<IEnumerable<Card>> GetCardsAsync()
        {
            return await context.Cards.ToListAsync();
        }

        public async Task<Card> GetCardAsync(int id)
        {
            return await context.Cards.FindAsync(id);
        }
        public async Task AddCardAsync(Card card)
        {
            if (card == null)
            {
                throw new ArgumentNullException(nameof(card));
            }

            await context.Cards.AddAsync(card);
            await context.SaveChangesAsync();
        }

        public async Task UpdateCardAsync(Card updatedCard)
        {
            if (updatedCard == null)
            {
                throw new ArgumentNullException(nameof(updatedCard));
            }

            context.Cards.Update(updatedCard);
            await context.SaveChangesAsync();
        }
        public async Task DeleteCardAsync(int id)
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

