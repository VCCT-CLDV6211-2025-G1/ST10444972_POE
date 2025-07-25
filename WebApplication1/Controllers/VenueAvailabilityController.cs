using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VenueAvailabilityController : ControllerBase
    {
        private readonly IVenueAvailabilityService _availabilityService;

        public VenueAvailabilityController(IVenueAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }

        [HttpGet("booked-dates/{venueId}")]
        public async Task<IActionResult> GetBookedDates(int venueId, [FromQuery] int? excludeEventId = null)
        {
            var bookedDates = await _availabilityService.GetBookedDates(venueId, excludeEventId);
            return Ok(bookedDates.Select(d => d.ToString("yyyy-MM-dd")));
        }

        [HttpGet("check")]
        public async Task<IActionResult> CheckAvailability([FromQuery] int venueId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] int? excludeEventId = null)
        {
            var isAvailable = await _availabilityService.IsVenueAvailable(venueId, startDate, endDate, excludeEventId);
            var conflicts = await _availabilityService.GetOverlappingEvents(venueId, startDate, endDate, excludeEventId);
            
            return Ok(new { isAvailable, conflicts });
        }
    }
}