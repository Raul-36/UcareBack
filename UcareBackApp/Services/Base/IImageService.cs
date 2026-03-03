using Microsoft.AspNetCore.Http;

namespace UcareBackApp.Services.ImageService
{
    public interface IImageService
    {
        Task<string?> AddImageAsync(IFormFile image, string name);
        Task DeleteImageAsync(string imageUrl);
        Task<string?> UpdateImageAsync(IFormFile image, string oldImageUrl);
    }
}
