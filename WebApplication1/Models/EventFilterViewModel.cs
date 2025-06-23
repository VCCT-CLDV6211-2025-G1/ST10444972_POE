using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class EventFilterViewModel
    {
        public string? SearchTerm { get; set; }
        
        [Display(Name = "Event Type")]
        public int? EventTypeId { get; set; }
        
        [Display(Name = "Venue")]
        public int? VenueId { get; set; }
        
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }
        
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }
        
        [Display(Name = "Status")]
        public string? Status { get; set; }

        // Collection properties for dropdown lists
        public IEnumerable<EventType>? EventTypes { get; set; }
        public IEnumerable<Venue>? Venues { get; set; }
    }
}
