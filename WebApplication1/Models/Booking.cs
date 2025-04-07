using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [Required]
        [Display(Name = "Event")]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual Event Event { get; set; } = null!;

        [Required]
        [Display(Name = "Venue")]
        public int VenueId { get; set; }

        [ForeignKey("VenueId")]
        public virtual Venue Venue { get; set; } = null!;

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; } = null!;

        [Required]
        public string Status { get; set; } = "pending";  // pending, confirmed, cancelled

        [Required]
        [StringLength(100)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Last Modified")]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;
    }
}
