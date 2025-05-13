using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Validation
{
    public class AllowedImageExtensionsAttribute : ValidationAttribute
    {
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly int _maxSizeInMb;

        public AllowedImageExtensionsAttribute(int maxSizeInMb = 5)
        {
            _maxSizeInMb = maxSizeInMb;
            ErrorMessage = $"Please upload an image file (allowed extensions: {string.Join(", ", _allowedExtensions)}) under {_maxSizeInMb}MB";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                
                if (!_allowedExtensions.Contains(extension))
                {
                    return new ValidationResult($"This file type is not allowed. Please upload {string.Join(", ", _allowedExtensions)}");
                }

                if (file.Length > _maxSizeInMb * 1024 * 1024)
                {
                    return new ValidationResult($"File size cannot exceed {_maxSizeInMb}MB");
                }

                // Verify it's actually an image
                try
                {
                    using var image = System.Drawing.Image.FromStream(file.OpenReadStream());
                    if (image.Width <= 0 || image.Height <= 0)
                    {
                        return new ValidationResult("The file is not a valid image");
                    }
                }
                catch
                {
                    return new ValidationResult("The file is not a valid image");
                }
            }

            return ValidationResult.Success;
        }
    }
}
