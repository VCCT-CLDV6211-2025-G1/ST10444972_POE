using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueAvailabilityController : ControllerBase
    {
        private readonly IVenueAvailabilityService _availabilityService;
        private readonly ILogger<VenueAvailabilityController> _logger;

        public VenueAvailabilityController(
            IVenueAvailabilityService availabilityService,
            ILogger<VenueAvailabilityController> logger)
        {
            _availabilityService = availabilityService;
            _logger = logger;
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckAvailability(
            [FromQuery] int venueId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate,
            [FromQuery] int? excludeEventId = null)
        {
            _logger.LogInformation(
                "API - Checking venue availability - VenueId: {VenueId}, Start: {StartDate}, End: {EndDate}, ExcludeEventId: {ExcludeEventId}",
                venueId, startDate, endDate, excludeEventId);

            try
            {
                if (startDate >= endDate)
                {
                    _logger.LogWarning(
                        "API - Invalid date range - Start: {StartDate} is not before End: {EndDate}",
                        startDate, endDate);
                    return BadRequest("Start date must be before end date");
                }

                if (startDate < DateTime.UtcNow)
                {
                    _logger.LogWarning(
                        "API - Start date in past - Start: {StartDate}, Current: {CurrentTime}",
                        startDate, DateTime.UtcNow);
                    return BadRequest("Start date cannot be in the past");
                }

                var overlappingEvents = await _availabilityService.GetOverlappingEvents(
                    venueId, startDate, endDate, excludeEventId);

                var response = new
                {
                    isAvailable = !overlappingEvents.Any(),
                    conflicts = overlappingEvents.Select(e => new
                    {
                        e.EventName,
                        e.StartDate,
                        e.EndDate
                    })
                };

                _logger.LogInformation(
                    "API - Availability check complete - VenueId: {VenueId}, IsAvailable: {IsAvailable}, ConflictCount: {ConflictCount}",
                    venueId, response.isAvailable, overlappingEvents.Count);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "API - Error checking venue availability - VenueId: {VenueId}, Start: {StartDate}, End: {EndDate}",
                    venueId, startDate, endDate);
                return StatusCode(500, "An error occurred while checking venue availability");
            }
        }
    }
}
