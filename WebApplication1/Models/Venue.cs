using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using WebApplication1.Validation;

namespace WebApplication1.Models
{
    public class Venue
    {
        [Key]
        public int VenueId { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Venue Name")]
        public string VenueName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        [Required]
        [Range(1, 100000)]
        public int Capacity { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = string.Empty;

        [Display(Name = "Venue Image")]
        [NotMapped]
        [AllowedImageExtensions(5)] // Max 5MB file size
        public IFormFile? ImageFile { get; set; }

        [Required]
        public VenueStatus Status { get; set; } = VenueStatus.Active;

        [Display(Name = "Last Modified")]
        [ScaffoldColumn(false)]
        public DateTime LastModified { get; set; } = DateTime.UtcNow;

        [Display(Name = "Created Date")]
        [ScaffoldColumn(false)]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public virtual ICollection<Event> Events { get; set; } = new List<Event>();
        public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
