-- Insert sample Clients
INSERT INTO Clients (ClientName, Email, Phone, Address, CreatedDate) VALUES
('John Smith', 'john.smith@email.com', '+27123456789', '123 Main Street, Johannesburg', GETDATE()),
('Sarah Johnson', 'sarah.j@email.com', '+27987654321', '456 Park Avenue, Cape Town', GETDATE()),
('Mike Brown', 'mike.brown@email.com', '+27765432198', '789 Beach Road, Durban', GETDATE());

-- Insert sample Venues
INSERT INTO Venues (VenueName, Location, Capacity, ImageUrl, Status, LastModified, CreatedDate) VALUES
('Grand Ballroom', 'Sandton Convention Centre', 500, 'venues/ballroom.jpg', 'Available', GETDATE(), GETDATE()),
('Garden Terrace', 'Cape Town Waterfront', 200, 'venues/garden.jpg', 'Available', GETDATE(), GETDATE()),
('Beach View Hall', 'Durban Beachfront', 300, 'venues/beachhall.jpg', 'Available', GETDATE(), GETDATE());

-- Insert sample Events
INSERT INTO Events (EventName, StartDate, EndDate, Description, VenueId, Status, LastModified, CreatedDate) VALUES
('Corporate Conference 2025', '2025-06-15 09:00:00', '2025-06-15 17:00:00', 'Annual corporate conference with keynote speakers', 1, 'Scheduled', GETDATE(), GETDATE()),
('Wedding Reception', '2025-07-20 14:00:00', '2025-07-20 23:00:00', 'Smith-Johnson Wedding Reception', 2, 'Confirmed', GETDATE(), GETDATE()),
('Tech Summit 2025', '2025-08-10 08:00:00', '2025-08-12 18:00:00', 'Technology and Innovation Summit', 3, 'Planning', GETDATE(), GETDATE());

-- Insert sample Bookings
INSERT INTO Bookings (EventId, VenueId, ClientId, Status, CreatedBy, CreatedDate, LastModified) VALUES
(1, 1, 1, 'Confirmed', 'Admin', GETDATE(), GETDATE()),
(2, 2, 2, 'Confirmed', 'Admin', GETDATE(), GETDATE()),
(3, 3, 3, 'Pending', 'Admin', GETDATE(), GETDATE());
