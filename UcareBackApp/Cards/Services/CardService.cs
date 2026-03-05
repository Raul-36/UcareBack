using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UcareBackApp.Cards.Dtos.Requests;
using UcareBackApp.Cards.Dtos.Responses;
using UcareBackApp.Cards.Entities;
using UcareBackApp.Cards.Repositories.Base;
using UcareBackApp.Cards.Services.Base;
using UcareBackApp.Services.ImageService;

namespace UcareBackApp.Cards.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository cardRepository;
        private readonly ICardAccessChecker cardAccessChecker;
        private readonly IImageService imageService;

        public CardService(ICardRepository cardRepository, IImageService imageService, ICardAccessChecker cardAccessChecker)
        {
            this.cardRepository = cardRepository;
            this.imageService = imageService;
            this.cardAccessChecker = cardAccessChecker;
        }

        public async Task<FullCardResponse> AddCardAsync(CreateCardRequest request, Guid userId)
        {
            if (request.Image is null)
                throw new BadHttpRequestException("Image is required");

            var newCard = new Card
            {
                Name = request.Name,
                Address = request.Address,
                Occupation = request.Occupation,
                Description = request.Description,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                UserId = userId,
                ImageUrl = await this.imageService.AddImageAsync(request.Image, $"{Guid.NewGuid()}.jpg")
            };
            var addedCard = await cardRepository.AddCardAsync(newCard);
            return new FullCardResponse
            {
                Id = addedCard.Id,
                Name = addedCard.Name,
                Address = addedCard.Address,
                Occupation = addedCard.Occupation,
                Description = addedCard.Description,
                Latitude = addedCard.Latitude,
                Longitude = addedCard.Longitude,
                UserId = addedCard.UserId,
                ImageUrl = addedCard.ImageUrl
            };
        }

        public async Task DeleteCardAsync(Guid id, Guid userId)
        {
            var hasAccess = await this.cardAccessChecker.HasAccessToCard(userId, id);
            if (hasAccess == false)
                throw new UnauthorizedAccessException("User does not have access to this card");

            var card = await cardRepository.GetCardAsync(id);
            if (card == null)
                return;
            if (string.IsNullOrEmpty(card.ImageUrl) == false)
                await this.imageService.DeleteImageAsync(card.ImageUrl);
            await cardRepository.DeleteCardAsync(id);
        }

        public async Task<FullCardResponse> GetCardAsync(Guid id)
        {
            var card = await cardRepository.GetCardAsync(id);
            if (card == null)
                throw new KeyNotFoundException("Card not found");

            return new FullCardResponse
            {
                Id = card.Id,
                Name = card.Name,
                Address = card.Address,
                Occupation = card.Occupation,
                Description = card.Description,
                Latitude = card.Latitude,
                Longitude = card.Longitude,
                UserId = card.UserId,
                ImageUrl = card.ImageUrl
            };
        }

        public async Task<IEnumerable<ShortCardResponse>> GetCardsAsync()
        {
            var cards = await cardRepository.GetCardsAsync();
            return cards.Select(card => new ShortCardResponse
            {
                Id = card.Id,
                Name = card.Name,
                Address = card.Address,
                Occupation = card.Occupation,
                ImageUrl = card.ImageUrl
            });
        }

        public async Task UpdateCardImageAsync(Guid cardId, IFormFile image, Guid userId)
        {
            var hasAccess = await this.cardAccessChecker.HasAccessToCard(userId, cardId);
            if (hasAccess == false)
                throw new UnauthorizedAccessException("User does not have access to this card");

            var card = await this.GetCardAsync(cardId);
            if (card == null)
                throw new KeyNotFoundException("Card not found");
            
            if(card.ImageUrl != null)
            {
                await this.imageService.UpdateImageAsync(image, card.ImageUrl );
            }
            else
            {
                var newImageUrl = await this.imageService.AddImageAsync(image, $"{cardId}.jpg");
                card.ImageUrl = newImageUrl;

                var cardEntity = new Card
                {
                    Name = card.Name,
                    Address = card.Address,
                    Occupation = card.Occupation,
                    Description = card.Description,
                    Latitude = card.Latitude,
                    Longitude = card.Longitude,
                    UserId = card.UserId,
                    ImageUrl = newImageUrl
                };
                await this.cardRepository.UpdateCardAsync(cardEntity);
            }
        }

        public async Task<FullCardResponse> UpdateCardInfoAsync(UpdateCardRequest request, Guid userId)
        {
            var hasAccess = await this.cardAccessChecker.HasAccessToCard(userId, request.Id);
            if (hasAccess == false)
                throw new UnauthorizedAccessException("User does not have access to this card");   

            var card = await this.GetCardAsync(request.Id);
            if (card == null)
                throw new KeyNotFoundException("Card not found");

            var cardEntity = new Card
            {
                Id = request.Id,
                Name = request.Name,
                Address = request.Address,
                Occupation = request.Occupation,
                Description = request.Description,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                UserId = userId,
                ImageUrl = card.ImageUrl
            };    
            
            var resultEntity = await cardRepository.UpdateCardAsync(cardEntity);
   
            return new FullCardResponse
            {
                Id = resultEntity.Id,
                Name = resultEntity.Name,
                Address = resultEntity.Address,
                Occupation = resultEntity.Occupation,
                Description = resultEntity.Description,
                Latitude = resultEntity.Latitude,
                Longitude = resultEntity.Longitude,
                UserId = resultEntity.UserId,
                ImageUrl = resultEntity.ImageUrl
            };
        }   
    }
}