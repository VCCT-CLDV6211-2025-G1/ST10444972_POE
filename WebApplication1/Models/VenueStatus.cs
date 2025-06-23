namespace WebApplication1.Models
{
    public enum VenueStatus
    {
        Active,
        Available, // Legacy status - use Active for new venues
        Maintenance,
        Inactive
    }
}
