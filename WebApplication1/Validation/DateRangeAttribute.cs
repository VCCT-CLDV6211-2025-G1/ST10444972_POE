using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Validation
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly string _endDatePropertyName;
        
        public DateRangeAttribute(string endDatePropertyName)
        {
            _endDatePropertyName = endDatePropertyName;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var startDate = (DateTime)value!;
            var endDateProperty = validationContext.ObjectType.GetProperty(_endDatePropertyName);
            
            if (endDateProperty == null)
            {
                return new ValidationResult($"Unknown property: {_endDatePropertyName}");
            }

            var endDate = (DateTime)endDateProperty.GetValue(validationContext.ObjectInstance)!;

            if (startDate >= endDate)
            {
                return new ValidationResult("Start date must be before end date");
            }

            // Ensure dates are not in the past
            if (startDate.Date < DateTime.UtcNow.Date)
            {
                return new ValidationResult("Start date cannot be in the past");
            }

            // Ensure the event duration is not too long (e.g., max 7 days)
            var duration = endDate - startDate;
            if (duration.TotalDays > 7)
            {
                return new ValidationResult("Event duration cannot exceed 7 days");
            }

            return ValidationResult.Success;
        }
    }
}
