using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebApplication1.Validation
{
    public class AllowedImageExtensionsAttribute : ValidationAttribute
    {
        private readonly int _maxSizeInMb;
        private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };

        public AllowedImageExtensionsAttribute(int maxSizeInMb = 5)
        {
            _maxSizeInMb = maxSizeInMb;
            ErrorMessage = $"Please upload a valid image file under {_maxSizeInMb}MB (.jpg, .jpeg, .png)";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                {
                    return new ValidationResult("This file type is not allowed. Please upload a valid image file (.jpg, .jpeg, .png)");
                }

                if (file.Length > _maxSizeInMb * 1024 * 1024)
                {
                    return new ValidationResult($"File size cannot exceed {_maxSizeInMb}MB");
                }
            }

            return ValidationResult.Success;
        }
    }
}
