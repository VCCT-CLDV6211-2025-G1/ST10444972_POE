using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IVenueAvailabilityService
    {
        Task<bool> IsVenueAvailable(int venueId, DateTime startDate, DateTime endDate, int? excludeEventId = null);
        Task<List<Event>> GetOverlappingEvents(int venueId, DateTime startDate, DateTime endDate, int? excludeEventId = null);
    }

    public class VenueAvailabilityService : IVenueAvailabilityService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<VenueAvailabilityService> _logger;

        public VenueAvailabilityService(ApplicationDbContext context, ILogger<VenueAvailabilityService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<bool> IsVenueAvailable(int venueId, DateTime startDate, DateTime endDate, int? excludeEventId = null)
        {
            var overlappingEvents = await GetOverlappingEvents(venueId, startDate, endDate, excludeEventId);
            return !overlappingEvents.Any();
        }

        public async Task<List<Event>> GetOverlappingEvents(int venueId, DateTime startDate, DateTime endDate, int? excludeEventId = null)
        {
            try
            {
                _logger.LogInformation(
                    "Checking venue availability - VenueId: {VenueId}, Start: {StartDate}, End: {EndDate}, ExcludeEventId: {ExcludeEventId}",
                    venueId, startDate, endDate, excludeEventId);

                var query = _context.Events
                    .Where(e => e.VenueId == venueId)
                    .Where(e => e.Status != "cancelled");

                if (excludeEventId.HasValue)
                {
                    query = query.Where(e => e.EventId != excludeEventId.Value);
                }

                var overlappingEvents = await query
                    .Where(e =>
                        (e.StartDate <= endDate && e.EndDate >= startDate) || // Event overlaps with requested period
                        (e.StartDate >= startDate && e.StartDate <= endDate) || // Event starts during requested period
                        (e.EndDate >= startDate && e.EndDate <= endDate)) // Event ends during requested period
                    .OrderBy(e => e.StartDate)
                    .ToListAsync();

                _logger.LogInformation("Found {Count} overlapping events", overlappingEvents.Count);
                
                if (overlappingEvents.Any())
                {
                    foreach (var evt in overlappingEvents)
                    {
                        _logger.LogInformation(
                            "Overlapping event found - EventId: {EventId}, Name: {EventName}, Start: {StartDate}, End: {EndDate}",
                            evt.EventId, evt.EventName, evt.StartDate, evt.EndDate);
                    }
                }

                return overlappingEvents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking venue availability");
                throw;
            }
        }
    }
}
