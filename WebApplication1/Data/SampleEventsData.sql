-- Insert additional sample events with various types, venues, and statuses
INSERT INTO Events (EventName, StartDate, EndDate, Description, VenueId, EventTypeId, Status, CreatedDate, LastModified) VALUES
-- Upcoming Conferences
('Tech Innovation Summit', '2025-07-15 09:00:00', '2025-07-16 17:00:00', 'Two-day technology innovation conference', 1, 
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Conference'), 'scheduled', GETDATE(), GETDATE()),
('Digital Marketing Conference', '2025-08-20 08:30:00', '2025-08-20 16:30:00', 'Digital marketing strategies and trends', 2,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Conference'), 'confirmed', GETDATE(), GETDATE()),

-- Past Conferences
('Data Science Symposium', '2025-05-10 09:00:00', '2025-05-10 17:00:00', 'Advanced data science and AI topics', 1,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Conference'), 'completed', GETDATE(), GETDATE()),

-- Weddings
('Thompson-Parker Wedding', '2025-09-14 15:00:00', '2025-09-14 23:00:00', 'Garden wedding ceremony and reception', 2,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Wedding'), 'scheduled', GETDATE(), GETDATE()),
('Williams-Davis Celebration', '2025-08-30 16:00:00', '2025-08-30 23:59:00', 'Elegant evening wedding celebration', 3,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Wedding'), 'confirmed', GETDATE(), GETDATE()),

-- Corporate Events
('Annual Shareholders Meeting', '2025-07-30 10:00:00', '2025-07-30 13:00:00', 'Company annual shareholders meeting', 1,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Corporate Event'), 'scheduled', GETDATE(), GETDATE()),
('Product Launch Gala', '2025-08-05 18:00:00', '2025-08-05 22:00:00', 'New product line launch event', 2,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Corporate Event'), 'confirmed', GETDATE(), GETDATE()),

-- Cancelled Events
('Business Leadership Summit', '2025-07-25 09:00:00', '2025-07-25 17:00:00', 'Leadership development conference', 3,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Conference'), 'cancelled', GETDATE(), GETDATE()),
('Johnson-Lee Wedding', '2025-08-02 14:00:00', '2025-08-02 22:00:00', 'Wedding ceremony and reception', 2,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Wedding'), 'cancelled', GETDATE(), GETDATE()),

-- Multi-day Events
('International Trade Show', '2025-09-01 09:00:00', '2025-09-03 18:00:00', 'International trade and commerce exhibition', 1,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Exhibition'), 'scheduled', GETDATE(), GETDATE()),
('Summer Music Festival', '2025-08-15 12:00:00', '2025-08-17 23:00:00', 'Three-day music festival', 3,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Concert'), 'confirmed', GETDATE(), GETDATE()),

-- Workshop and Social Events
('Startup Networking Event', '2025-07-10 18:00:00', '2025-07-10 21:00:00', 'Networking event for startup entrepreneurs', 2,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Social Gathering'), 'scheduled', GETDATE(), GETDATE()),
('Charity Concert Series', '2025-08-25 19:00:00', '2025-08-25 23:00:00', 'Live music concert for charity', 3,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Charity Event'), 'scheduled', GETDATE(), GETDATE()),

-- Technical Events
('AI & ML Workshop 2025', '2025-07-22 09:00:00', '2025-07-22 17:00:00', 'Artificial Intelligence & Machine Learning workshop', 1,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Workshop'), 'scheduled', GETDATE(), GETDATE()),
('Team Building Day', '2025-08-08 10:00:00', '2025-08-08 16:00:00', 'Corporate team building activities', 2,
    (SELECT TOP 1 EventTypeId FROM EventTypes WHERE Name = 'Corporate Event'), 'confirmed', GETDATE(), GETDATE());

-- Insert event bookings
INSERT INTO Bookings (EventId, VenueId, ClientId, Status, CreatedBy, CreatedDate, LastModified)
SELECT e.EventId, e.VenueId, 
    (SELECT TOP 1 ClientId FROM Clients ORDER BY NEWID()), -- Random client
    CASE 
        WHEN e.Status = 'confirmed' THEN 'Confirmed'
        WHEN e.Status = 'cancelled' THEN 'Cancelled'
        ELSE 'Pending'
    END,
    'System', GETDATE(), GETDATE()
FROM Events e
WHERE e.EventId NOT IN (SELECT EventId FROM Bookings);
