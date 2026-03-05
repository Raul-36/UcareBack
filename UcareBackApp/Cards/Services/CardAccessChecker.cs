using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using UcareBackApp.Cards.Repositories.Base;
using UcareBackApp.Cards.Services.Base;
using UcareBackApp.Identity.Entities;

namespace UcareBackApp.Cards.Services
{
    public class CardAccessChecker : ICardAccessChecker
    {
        private readonly ICardRepository cardRepository;
        private readonly UserManager<UcareUser> userManager;
        public CardAccessChecker(ICardRepository cardRepository, UserManager<UcareUser> userManager)
        {
            this.cardRepository = cardRepository;
            this.userManager = userManager;
        }
        public async Task<bool> HasAccessToCard(Guid userId, Guid cardId)
        {
                var card = await cardRepository.GetCardAsync(cardId);
                if (card == null)
                    return false;

                var user = await userManager.FindByIdAsync(userId.ToString());
                if (user == null)
                    return false;

                var isAdmin = await userManager.IsInRoleAsync(user, "admin");
                if (isAdmin)
                    return true;
                    
                return card.UserId == userId;
        }
    }
}