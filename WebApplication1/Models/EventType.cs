using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class EventType
    {
        public int EventTypeId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Event Type Name")]
        public required string Name { get; set; }

        [Required]
        [StringLength(500)]
        public required string Description { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        // Navigation property
        public required virtual ICollection<Event> Events { get; set; }
    }
}
