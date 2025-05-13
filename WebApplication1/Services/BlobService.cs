using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly ImageService _imageService;
        private readonly ILogger<BlobService> _logger;

        public BlobService(IConfiguration configuration, ImageService imageService, ILogger<BlobService> logger)
        {
            _blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("BlobStorage"));
            _containerName = "images";
            _imageService = imageService;
            _logger = logger;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            try
            {
                _logger.LogInformation("Starting image upload process for file: {FileName}", file.FileName);
                
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var blobName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var blobClient = containerClient.GetBlobClient(blobName);

                // Resize the image
                _logger.LogInformation("Resizing image");
                using var stream = file.OpenReadStream();
                var resizedImageBytes = await _imageService.ResizeImageAsync(stream);
                
                using var resizedStream = new MemoryStream(resizedImageBytes);
                await blobClient.UploadAsync(resizedStream, new BlobHttpHeaders 
                { 
                    ContentType = "image/jpeg" // We're converting all images to JPEG in ImageService
                });

                _logger.LogInformation("Image uploaded successfully. Blob name: {BlobName}", blobName);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image: {FileName}", file.FileName);
                throw;
            }
        }

        public async Task DeleteImageAsync(string blobUrl)
        {
            if (string.IsNullOrEmpty(blobUrl)) return;

            try
            {
                _logger.LogInformation("Attempting to delete image: {BlobUrl}", blobUrl);
                
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                var uri = new Uri(blobUrl);
                var blobName = Path.GetFileName(uri.LocalPath);
                await containerClient.DeleteBlobIfExistsAsync(blobName);
                
                _logger.LogInformation("Image deleted successfully: {BlobName}", blobName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image: {BlobUrl}", blobUrl);
                throw;
            }
        }
    }
}
