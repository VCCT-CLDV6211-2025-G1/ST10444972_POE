using SkiaSharp;

namespace WebApplication1.Services
{
    public class ImageService
    {
        private const int MaxHeight = 1080;
        private const double TargetAspectRatio = 16.0 / 9.0;
        private const double AspectRatioTolerance = 0.01; // Allow 1% tolerance

        public async Task<byte[]> SaveImageAsync(Stream imageStream)
        {
            using var inputStream = new MemoryStream();
            await imageStream.CopyToAsync(inputStream);
            inputStream.Position = 0;

            using var bitmap = SKBitmap.Decode(inputStream);
            if (bitmap == null)
                throw new InvalidOperationException("Could not decode image");

            ValidateImageDimensions(bitmap);

            // Return the original image data if validation passes
            inputStream.Position = 0;
            return inputStream.ToArray();
        }

        public bool IsImageFile(string fileName)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension);
        }

        private void ValidateImageDimensions(SKBitmap bitmap)
        {
            if (bitmap.Height > MaxHeight)
                throw new ArgumentException($"Image height must not exceed {MaxHeight}px");

            double aspectRatio = (double)bitmap.Width / bitmap.Height;
            double aspectRatioDifference = Math.Abs(aspectRatio - TargetAspectRatio);

            if (aspectRatioDifference > AspectRatioTolerance)
                throw new ArgumentException("Image must have a 16:9 aspect ratio");
        }
    }
}
