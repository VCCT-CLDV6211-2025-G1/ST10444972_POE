using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<VenueAvailabilityPeriod> VenueAvailabilityPeriods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Venue>()
                        .Property(v => v.Status)
                        .HasConversion<string>();

            // Configure one-to-many relationship between Venue and Event
            modelBuilder.Entity<Event>()
                .HasOne(e => e.Venue)
                .WithMany(v => v.Events)
                .HasForeignKey(e => e.VenueId)  
                .OnDelete(DeleteBehavior.Restrict);  // Prevent  cascade delete

            // Configure one-to-one relationship between Event and Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithOne(e => e.Booking)
                .HasForeignKey<Booking>(b => b.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between Venue and Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany(v => v.Bookings)
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between Client and Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Client)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between EventType and Event
            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventType)
                .WithMany(et => et.Events)
                .HasForeignKey(e => e.EventTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure one-to-many relationship between Venue and VenueAvailabilityPeriod
            modelBuilder.Entity<VenueAvailabilityPeriod>()
                .HasOne(v => v.Venue)
                .WithMany(v => v.AvailabilityPeriods)
                .HasForeignKey(v => v.VenueId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
