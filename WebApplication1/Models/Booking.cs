using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [Display(Name = "Booking Date")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Number of Attendees")]
        [Range(1, 10000)]
        public int NumberOfAttendees { get; set; }

        [Required]
        [Display(Name = "Total Cost")]
        [DataType(DataType.Currency)]
        public decimal TotalCost { get; set; }

        [Required]
        public int EventId { get; set; }
        public virtual Event? Event { get; set; }

        [Required]
        public int ClientId { get; set; }
        public virtual Client? Client { get; set; }
    }
}
