using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IVenueAvailabilityService _availabilityService;
        private readonly IEventFilterService _filterService;
        private readonly ILogger<EventsController> _logger;

        public EventsController(
            ApplicationDbContext context,
            IVenueAvailabilityService availabilityService,
            IEventFilterService filterService,
            ILogger<EventsController> logger)
        {
            _context = context;
            _availabilityService = availabilityService;
            _filterService = filterService;
            _logger = logger;
        }

        public async Task<IActionResult> Index([FromQuery]EventFilterViewModel filter)
        {
            try
            {
                _logger.LogInformation("Receiving filter parameters: {@Filter}", filter);

                // Initialize filter if null
                filter ??= new EventFilterViewModel();

                // Populate dropdowns and apply filters
                filter = await _filterService.PopulateFilterDropdowns(filter);
                var events = await _filterService.FilterEvents(filter);

                _logger.LogInformation(
                    "Filter applied successfully - Found {Count} events matching criteria", 
                    events.Count());

                return View((Events: events, Filter: filter));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving events");
                return View("Error");
            }
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .Include(e => e.Venue)
                .Include(e => e.EventType)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Venues = await _context.Venues
                .Where(v => v.Status == VenueStatus.Active)
                .OrderBy(v => v.VenueName)
                .ToListAsync();

            ViewBag.EventTypes = await _context.EventTypes
                .Where(et => et.IsActive)
                .OrderBy(et => et.Name)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventName,StartDate,EndDate,Description,VenueId,EventTypeId")] Event @event)
        {
            _logger.LogInformation(
                "Attempting to create event - Name: {EventName}, Start: {StartDate}, End: {EndDate}, VenueId: {VenueId}",
                @event.EventName, @event.StartDate, @event.EndDate, @event.VenueId);

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model state is invalid: {Errors}", 
                        string.Join(", ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)));
                }
                else
                {
                    _context.Add(@event);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Event created successfully: {EventName}", @event.EventName);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating event: {EventName}", @event.EventName);
                ModelState.AddModelError("", "An error occurred while creating the event. Please try again.");
            }

            ViewBag.Venues = await _context.Venues
                .Where(v => v.Status == VenueStatus.Active)
                .OrderBy(v => v.VenueName)
                .ToListAsync();

            if (@event.VenueId.HasValue)
            {
                var overlappingEvents = await _availabilityService.GetOverlappingEvents(
                    @event.VenueId.Value,
                    @event.StartDate,
                    @event.EndDate
                );

                if (overlappingEvents.Any())
                {
                    ViewBag.OverlappingEvents = overlappingEvents;
                }
            }

            return View(@event);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            if (!@event.CanModify())
            {
                return BadRequest("Cannot modify cancelled events or events that have already started.");
            }

            ViewBag.Venues = await _context.Venues
                .Where(v => v.Status == VenueStatus.Active)
                .OrderBy(v => v.VenueName)
                .ToListAsync();

            ViewBag.EventTypes = await _context.EventTypes
                .OrderBy(et => et.Name)
                .ToListAsync();

            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,EventName,StartDate,EndDate,Description,VenueId,Status,EventTypeId")] Event @event)
        {
            _logger.LogInformation(
                "Attempting to edit event - Id: {EventId}, Name: {EventName}, Start: {StartDate}, End: {EndDate}, VenueId: {VenueId}",
                id, @event.EventName, @event.StartDate, @event.EndDate, @event.VenueId);

            if (id != @event.EventId)
            {
                _logger.LogWarning("Event ID mismatch - URL: {UrlId}, Model: {ModelId}", id, @event.EventId);
                return NotFound();
            }

            var existingEvent = await _context.Events.FindAsync(id);
            if (existingEvent == null)
            {
                _logger.LogWarning("Event not found - Id: {EventId}", id);
                return NotFound();
            }

            if (!existingEvent.CanModify())
            {
                _logger.LogWarning(
                    "Cannot modify event - Id: {EventId}, Status: {Status}, StartDate: {StartDate}",
                    id, existingEvent.Status, existingEvent.StartDate);
                return BadRequest("Cannot modify cancelled events or events that have already started.");
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model state is invalid: {Errors}", 
                        string.Join(", ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)));
                }
                else
                {
                    existingEvent.EventName = @event.EventName;
                    existingEvent.StartDate = @event.StartDate;
                    existingEvent.EndDate = @event.EndDate;
                    existingEvent.Description = @event.Description;
                    existingEvent.VenueId = @event.VenueId;
                    existingEvent.Status = @event.Status;
                    existingEvent.EventTypeId = @event.EventTypeId;
                    existingEvent.LastModified = DateTime.UtcNow;

                    _context.Update(existingEvent);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Event updated successfully: {EventName}", @event.EventName);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!EventExists(@event.EventId))
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error updating event: {EventName}", @event.EventName);
                    ModelState.AddModelError("", "The event was modified by another user. Please refresh and try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating event: {EventName}", @event.EventName);
                ModelState.AddModelError("", "An error occurred while updating the event. Please try again.");
            }

            ViewBag.Venues = await _context.Venues
                .Where(v => v.Status == VenueStatus.Active)
                .OrderBy(v => v.VenueName)
                .ToListAsync();

            if (@event.VenueId.HasValue)
            {
                var overlappingEvents = await _availabilityService.GetOverlappingEvents(
                    @event.VenueId.Value,
                    @event.StartDate,
                    @event.EndDate,
                    @event.EventId
                );

                if (overlappingEvents.Any())
                {
                    ViewBag.OverlappingEvents = overlappingEvents;
                }
            }

            return View(@event);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            _logger.LogInformation("Attempting to cancel event - Id: {EventId}", id);

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                _logger.LogWarning("Event not found for cancellation - Id: {EventId}", id);
                return NotFound();
            }

            if (!@event.CanModify())
            {
                _logger.LogWarning(
                    "Cannot cancel event - Id: {EventId}, Status: {Status}, StartDate: {StartDate}",
                    id, @event.Status, @event.StartDate);
                return BadRequest("Cannot cancel events that have already started.");
            }

            try
            {
                @event.Status = "cancelled";
                @event.LastModified = DateTime.UtcNow;
                _context.Update(@event);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation(
                    "Event cancelled successfully - Id: {EventId}, Name: {EventName}, Start: {StartDate}",
                    @event.EventId, @event.EventName, @event.StartDate);
                
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error cancelling event - Id: {EventId}, Name: {EventName}", 
                    id, @event.EventName);
                return BadRequest("The event was modified by another user. Please refresh and try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling event - Id: {EventId}, Name: {EventName}", 
                    id, @event.EventName);
                return BadRequest("An error occurred while cancelling the event. Please try again.");
            }
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
