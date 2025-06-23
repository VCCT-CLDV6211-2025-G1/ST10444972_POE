using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using WebApplication1.Services;

namespace WebApplication1.Validation
{
    public class VenueAvailabilityAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var venueId = (int)value;
            var eventModel = validationContext.ObjectInstance as Models.Event;

            if (eventModel == null)
            {
                return new ValidationResult("VenueAvailabilityAttribute can only be used with Event model");
            }

            // Skip validation if no dates are set
            if (eventModel.StartDate == default || eventModel.EndDate == default)
            {
                return ValidationResult.Success;
            }

            var availabilityService = validationContext.GetService<IVenueAvailabilityService>();
            if (availabilityService == null)
            {
                throw new InvalidOperationException("IVenueAvailabilityService not registered");
            }

            var isAvailable = availabilityService.IsVenueAvailable(
                venueId,
                eventModel.StartDate,
                eventModel.EndDate,
                eventModel.EventId // Exclude current event when editing
            ).GetAwaiter().GetResult();

            if (!isAvailable)
            {
                var overlappingEvents = availabilityService.GetOverlappingEvents(
                    venueId,
                    eventModel.StartDate,
                    eventModel.EndDate,
                    eventModel.EventId
                ).GetAwaiter().GetResult();

                var conflictMessage = overlappingEvents.Count == 1
                    ? $"Venue is already booked for event '{overlappingEvents[0].EventName}' during this time"
                    : $"Venue is already booked for {overlappingEvents.Count} events during this time";

                return new ValidationResult(conflictMessage);
            }

            return ValidationResult.Success;
        }
    }
}
