using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IEventFilterService
    {
        Task<IEnumerable<Event>> FilterEvents(EventFilterViewModel filter);
        Task<EventFilterViewModel> PopulateFilterDropdowns(EventFilterViewModel filter);
    }

    public class EventFilterService : IEventFilterService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EventFilterService> _logger;

        public EventFilterService(ApplicationDbContext context, ILogger<EventFilterService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Event>> FilterEvents(EventFilterViewModel filter)
        {
            try
            {
                _logger.LogInformation("Starting to filter events with parameters: {@Filter}", filter);

                var query = _context.Events
                    .Include(e => e.Venue)
                    .Include(e => e.EventType)
                    .AsQueryable();

                // Apply filters
                if (!string.IsNullOrWhiteSpace(filter?.SearchTerm))
                {
                    var searchTerm = filter.SearchTerm.ToLower();
                    query = query.Where(e => 
                        EF.Functions.Like(e.EventName.ToLower(), $"%{searchTerm}%") ||
                        EF.Functions.Like(e.Description.ToLower(), $"%{searchTerm}%") ||
                        EF.Functions.Like(e.Venue.VenueName.ToLower(), $"%{searchTerm}%")
                    );
                    _logger.LogDebug("Applied search term filter: {SearchTerm}", searchTerm);
                }

                if (filter?.EventTypeId.HasValue == true)
                {
                    query = query.Where(e => e.EventTypeId == filter.EventTypeId.Value);
                    _logger.LogDebug("Applied event type filter: {EventTypeId}", filter.EventTypeId);
                }

                if (filter?.VenueId.HasValue == true)
                {
                    query = query.Where(e => e.VenueId == filter.VenueId.Value);
                    _logger.LogDebug("Applied venue filter: {VenueId}", filter.VenueId);
                }

                if (filter?.StartDate.HasValue == true)
                {
                    var startDate = filter.StartDate.Value.Date;
                    query = query.Where(e => e.StartDate.Date >= startDate);
                    _logger.LogDebug("Applied start date filter: {StartDate}", startDate);
                }

                if (filter?.EndDate.HasValue == true)
                {
                    var endDate = filter.EndDate.Value.Date;
                    query = query.Where(e => e.EndDate.Date <= endDate);
                    _logger.LogDebug("Applied end date filter: {EndDate}", endDate);
                }

                if (!string.IsNullOrWhiteSpace(filter?.Status))
                {
                    query = query.Where(e => e.Status.ToLower() == filter.Status.ToLower());
                    _logger.LogDebug("Applied status filter: {Status}", filter.Status);
                }

                // Order by date
                query = query.OrderByDescending(e => e.StartDate);

                var result = await query.ToListAsync();
                _logger.LogInformation("Filter applied successfully. Found {Count} events", result.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error filtering events with parameters: {@Filter}", filter);
                throw;
            }
        }

        public async Task<EventFilterViewModel> PopulateFilterDropdowns(EventFilterViewModel filter)
        {
            filter.EventTypes = await _context.EventTypes
                .Where(et => et.IsActive)
                .OrderBy(et => et.Name)
                .ToListAsync();

            filter.Venues = await _context.Venues
                .Where(v => v.Status == VenueStatus.Active || v.Status == VenueStatus.Available)
                .OrderBy(v => v.VenueName)
                .ToListAsync();

            return filter;
        }
    }
}
