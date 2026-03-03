using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace UcareBackApp.Services.ImageService
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment hostingEnvironment;

        public ImageService(IWebHostEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;
        }

        public async Task<string?> AddImageAsync(IFormFile image, string name)
        {
            if (image == null || image.Length == 0)
                throw new BadHttpRequestException("Image file is required");

            var uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, name);

            if (File.Exists(filePath))
                throw new InvalidOperationException($"File with name '{name}' already exists");

            using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
            {
                await image.CopyToAsync(fileStream);
            }

            return "/images/" + name;
        }

        public Task DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                return Task.CompletedTask;
            }

            var imagePath = Path.Combine(hostingEnvironment.WebRootPath, imageUrl.TrimStart('/'));

            if (File.Exists(imagePath))
            {
                File.Delete(imagePath);
            }

            return Task.CompletedTask;
        }

        public async Task<string?> UpdateImageAsync(IFormFile image, string oldImageUrl)
        {
            if (!string.IsNullOrEmpty(oldImageUrl))
            {
                await DeleteImageAsync(oldImageUrl);
            }
            var fileName = Path.GetFileName(oldImageUrl);
            return await AddImageAsync(image, fileName);
        }
    }
}
