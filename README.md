# EventEase Project Overview

## Project Structure
EventEase is an ASP.NET Core MVC application designed to manage events, venues, clients, and bookings. The application follows a clean architecture pattern with the following components:

### Models
- **Event**: Represents event details including name, dates, and descriptions
- **Venue**: Handles venue information with capacity and location tracking
- **Client**: Manages client information and contact details
- **Booking**: Links events, venues, and clients together for event bookings

### Controllers
- **EventsController**: Handles event CRUD operations
- **VenuesController**: Manages venue-related operations
- **ClientsController**: Handles client management
- **BookingsController**: Coordinates the booking process
- **HomeController**: Manages the main landing pages

### Database
The application uses Entity Framework Core with a Code-First approach, demonstrated by:
- Initial migration (20250407070344_InitialCreate.cs)
- SQL Server configuration in production
- In-memory database support for development

## Azure Setup Progress

### Database Configuration
1. Created Azure SQL Server instance: `event-ease-server-st10444972.database.windows.net`
2. Attempted database user setup with credentials:
   - Username: ST10444972
   - Database: EventEaseDb

### Connection Issues Encountered
1. Initial connection string configuration in appsettings.json
2. Added retry logic for transient SQL connection errors
3. Implemented environment-specific database configurations:
   - Development: In-memory database
   - Production: Azure SQL Server

### Current Status
The application has been developed with full functionality but faces deployment challenges:
1. Database access issues for user 'ST10444972'
2. Need to complete database creation and permission setup in Azure
3. Connection string configuration requires verification

## Next Steps Needed
1. Verify Azure SQL Server firewall rules
2. Create EventEaseDb database in Azure
3. Grant appropriate permissions to ST10444972 user
4. Validate connection string format and credentials
5. Complete initial schema deployment through EF Core migrations

## Application Features
1. Event Management
   - Create, edit, and delete events
   - Track event status and details
   - Associate events with venues

2. Venue Management
   - Track venue capacity and availability
   - Manage venue status
   - Location tracking

3. Client Management
   - Client registration
   - Contact information tracking
   - Booking history

4. Booking System
   - Create and manage event bookings
   - Link clients, events, and venues
   - Track booking status

## Technical Implementation
- ASP.NET Core MVC architecture
- Entity Framework Core for data access
- Azure SQL Database for production data storage
- In-memory database support for development
- Bootstrap for responsive UI
- jQuery for client-side interactions
