using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IVenueAvailabilityService
    {
        Task<bool> IsVenueAvailable(int venueId, DateTime startDate, DateTime endDate, int? excludeEventId = null);
        Task<List<Event>> GetOverlappingEvents(int venueId, DateTime startDate, DateTime endDate, int? excludeEventId = null);
        Task<List<DateTime>> GetBookedDates(int venueId, int? excludeEventId = null);
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
            // Check if any day in the date range is already booked
            var bookedDates = await GetBookedDates(venueId, excludeEventId);
            var eventDates = GetDateRange(startDate.Date, endDate.Date);
            
            return !eventDates.Any(date => bookedDates.Contains(date));
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

                // Check for date conflicts (any day overlap)
                var eventDates = GetDateRange(startDate.Date, endDate.Date);
                var overlappingEvents = await query
                    .Where(e => eventDates.Any(date => 
                        date >= e.StartDate.Date && date <= e.EndDate.Date))
                    .OrderBy(e => e.StartDate)
                    .ToListAsync();

                _logger.LogInformation("Found {Count} overlapping events", overlappingEvents.Count);
                
                return overlappingEvents;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking venue availability");
                throw;
            }
        }

        public async Task<List<DateTime>> GetBookedDates(int venueId, int? excludeEventId = null)
        {
            var query = _context.Events
                .Where(e => e.VenueId == venueId)
                .Where(e => e.Status != "cancelled");

            if (excludeEventId.HasValue)
            {
                query = query.Where(e => e.EventId != excludeEventId.Value);
            }

            var events = await query.ToListAsync();
            var bookedDates = new List<DateTime>();

            foreach (var evt in events)
            {
                bookedDates.AddRange(GetDateRange(evt.StartDate.Date, evt.EndDate.Date));
            }

            return bookedDates.Distinct().ToList();
        }

        private static List<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            var dates = new List<DateTime>();
            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dates.Add(date);
            }
            return dates;
        }
    }
}
