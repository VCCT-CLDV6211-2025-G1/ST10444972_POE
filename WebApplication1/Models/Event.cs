using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication1.Validation;

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
        [DateRange("EndDate")] // Custom validation to ensure StartDate is before EndDate
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Venue")]
        [VenueAvailability] // Custom validation to check venue availability
        public int? VenueId { get; set; }  // Nullable to allow event creation before venue assignment

        [ForeignKey("VenueId")]
        public virtual Venue? Venue { get; set; }

        [Required]
        public string Status { get; set; } = "pending_venue";  // pending_venue, confirmed, cancelled

        [Display(Name = "Last Modified")]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Display(Name = "Event Type")]
        public int EventTypeId { get; set; }

        [ForeignKey("EventTypeId")]
        public virtual EventType EventType { get; set; }

        // Navigation property
        public virtual Booking? Booking { get; set; }

        // Helper method to check if dates overlap with another event
        public bool OverlapsWith(Event other)
        {
            return (StartDate <= other.EndDate && EndDate >= other.StartDate) ||
                   (StartDate >= other.StartDate && StartDate <= other.EndDate) ||
                   (EndDate >= other.StartDate && EndDate <= other.EndDate);
        }

        // Helper method to check if the event can be modified
        public bool CanModify()
        {
            return Status != "cancelled" && StartDate > DateTime.UtcNow;
        }
    }
}
