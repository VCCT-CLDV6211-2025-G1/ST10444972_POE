using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BookingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BookingsController> _logger;

        public BookingsController(ApplicationDbContext context, ILogger<BookingsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchString, string searchBy = "booking")
        {
            _logger.LogInformation("Searching bookings with searchString: {SearchString}, searchBy: {SearchBy}", 
                searchString, searchBy);

            var bookingsQuery = _context.Bookings
                .Include(b => b.Event)
                .Include(b => b.Event.Venue)
                .Include(b => b.Client)
                .Select(b => new BookingViewModel
                {
                    BookingId = b.BookingId,
                    EventName = b.Event.EventName,
                    VenueName = b.Event.Venue.VenueName,
                    VenueLocation = b.Event.Venue.Location,
                    EventDate = b.Event.EventDate,
                    BookingDate = b.BookingDate,
                    NumberOfAttendees = b.NumberOfAttendees,
                    TotalCost = b.TotalCost,
                    ClientName = b.Client.Name,
                    ClientEmail = b.Client.Email,
                    VenueImageUrl = b.Event.Venue.ImageUrl
                });

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.Trim().ToLower();
                switch (searchBy.ToLower())
                {
                    case "event":
                        bookingsQuery = bookingsQuery.Where(b => 
                            b.EventName.ToLower().Contains(searchString));
                        break;
                    case "booking":
                    default:
                        bookingsQuery = bookingsQuery.Where(b => 
                            b.BookingId.ToString().Contains(searchString));
                        break;
                }
            }

            var bookings = await bookingsQuery
                .OrderByDescending(b => b.BookingDate)
                .ToListAsync();

            // Pass the search parameters back to the view
            ViewData["CurrentSearchString"] = searchString;
            ViewData["CurrentSearchBy"] = searchBy;

            return View(bookings);
        }

        // Other actions remain the same...
    }
}
