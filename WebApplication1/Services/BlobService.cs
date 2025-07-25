using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Services
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;
        private readonly ILogger<BlobService> _logger;

        public BlobService(IConfiguration configuration, ILogger<BlobService> logger)
        {
            var connectionString = configuration.GetConnectionString("BlobStorage");
            if (!string.IsNullOrEmpty(connectionString))
            {
                _blobServiceClient = new BlobServiceClient(connectionString);
            }
            _containerName = "images";
            _logger = logger;
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (_blobServiceClient == null)
            {
                _logger.LogWarning("Blob storage not configured. Skipping image upload.");
                return string.Empty;
            }

            try
            {
                _logger.LogInformation("Starting image upload process for file: {FileName}", file.FileName);
                
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
                await containerClient.SetAccessPolicyAsync(PublicAccessType.Blob);
                
                var blobName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var blobClient = containerClient.GetBlobClient(blobName);

                using var stream = file.OpenReadStream();
                await blobClient.UploadAsync(stream, new BlobHttpHeaders 
                { 
                    ContentType = file.ContentType
                });

                _logger.LogInformation("Image uploaded successfully. Blob name: {BlobName}", blobName);
                return blobClient.Uri.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image: {FileName}", file.FileName);
                return string.Empty;
            }
        }

        public async Task DeleteImageAsync(string blobUrl)
        {
            if (string.IsNullOrEmpty(blobUrl) || _blobServiceClient == null) return;

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
            }
        }
    }
}
