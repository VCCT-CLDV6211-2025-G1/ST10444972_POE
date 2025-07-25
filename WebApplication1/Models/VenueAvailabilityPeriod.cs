using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class VenueAvailabilityPeriod
    {
        [Key]
        public int VenueAvailabilityPeriodId { get; set; }

        [Required]
        public int VenueId { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }

        [Required]
        [StringLength(200)]
        public required string Reason { get; set; }

        [ForeignKey("VenueId")]
        public required virtual Venue Venue { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
