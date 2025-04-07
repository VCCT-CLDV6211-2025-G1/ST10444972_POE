using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Event Name")]
        public string EventName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Venue")]
        public int? VenueId { get; set; }  // Nullable to allow event creation before venue assignment

        [ForeignKey("VenueId")]
        public virtual Venue? Venue { get; set; }

        [Required]
        public string Status { get; set; } = "pending_venue";  // pending_venue, confirmed, cancelled

        [Display(Name = "Last Modified")]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation property
        public virtual Booking? Booking { get; set; }
    }
}
