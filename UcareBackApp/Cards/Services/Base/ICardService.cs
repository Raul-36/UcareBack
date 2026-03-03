using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UcareBackApp.Cards.Dtos.Requests;
using UcareBackApp.Cards.Dtos.Responses;

namespace UcareBackApp.Cards.Services.Base
{
    public interface ICardService
    {
        public Task<IEnumerable<ShortCardResponse>> GetCardsAsync();
        public Task<FullCardResponse> GetCardAsync(Guid id);
        public Task<FullCardResponse> AddCardAsync(CreateCardRequest card, Guid userId);
        public Task<FullCardResponse> UpdateCardInfoAsync(UpdateCardRequest request, Guid userId);
        public Task UpdateCardImageAsync(Guid cardId, IFormFile image, Guid userId);
        public Task DeleteCardAsync(Guid id, Guid userId);   
    }
}