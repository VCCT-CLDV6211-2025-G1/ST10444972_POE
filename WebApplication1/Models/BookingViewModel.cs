using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }

        [Display(Name = "Event Name")]
        public string EventName { get; set; } = string.Empty;

        [Display(Name = "Venue Name")]
        public string VenueName { get; set; } = string.Empty;

        [Display(Name = "Venue Location")]
        public string VenueLocation { get; set; } = string.Empty;

        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        [Display(Name = "Number of Attendees")]
        public int NumberOfAttendees { get; set; }

        [Display(Name = "Total Cost")]
        [DataType(DataType.Currency)]
        public decimal TotalCost { get; set; }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; } = string.Empty;

        [Display(Name = "Client Email")]
        public string ClientEmail { get; set; } = string.Empty;

        [Display(Name = "Venue Image")]
        public string VenueImageUrl { get; set; } = string.Empty;
    }
}
