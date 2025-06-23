-- Create Clients table
CREATE TABLE [Clients] (
    [ClientId] INT IDENTITY(1,1) NOT NULL,
    [ClientName] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [Phone] NVARCHAR(20) NOT NULL,
    [Address] NVARCHAR(200) NOT NULL,
    [CreatedDate] DATETIME2 NOT NULL,
    CONSTRAINT [PK_Clients] PRIMARY KEY ([ClientId])
);

-- Create EventTypes table
CREATE TABLE [EventTypes] (
    [EventTypeId] INT IDENTITY(1,1) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(500) NOT NULL,
    [IsActive] BIT NOT NULL,
    CONSTRAINT [PK_EventTypes] PRIMARY KEY ([EventTypeId])
);

-- Create Venues table
CREATE TABLE [Venues] (
    [VenueId] INT IDENTITY(1,1) NOT NULL,
    [VenueName] NVARCHAR(100) NOT NULL,
    [Location] NVARCHAR(200) NOT NULL,
    [Capacity] INT NOT NULL,
    [ImageUrl] NVARCHAR(MAX) NOT NULL,
    [Status] NVARCHAR(MAX) NOT NULL,
    [LastModified] DATETIME2 NOT NULL,
    [CreatedDate] DATETIME2 NOT NULL,
    CONSTRAINT [PK_Venues] PRIMARY KEY ([VenueId])
);

-- Create Events table
CREATE TABLE [Events] (
    [EventId] INT IDENTITY(1,1) NOT NULL,
    [EventName] NVARCHAR(100) NOT NULL,
    [StartDate] DATETIME2 NOT NULL,
    [EndDate] DATETIME2 NOT NULL,
    [Description] NVARCHAR(500) NOT NULL,
    [VenueId] INT NULL,
    [EventTypeId] INT NOT NULL,
    [Status] NVARCHAR(MAX) NOT NULL,
    [LastModified] DATETIME2 NOT NULL,
    [CreatedDate] DATETIME2 NOT NULL,
    CONSTRAINT [PK_Events] PRIMARY KEY ([EventId]),
    CONSTRAINT [FK_Events_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues]([VenueId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Events_EventTypes_EventTypeId] FOREIGN KEY ([EventTypeId]) REFERENCES [EventTypes]([EventTypeId]) ON DELETE NO ACTION
);

-- Create Bookings table
CREATE TABLE [Bookings] (
    [BookingId] INT IDENTITY(1,1) NOT NULL,
    [EventId] INT NOT NULL,
    [VenueId] INT NOT NULL,
    [ClientId] INT NOT NULL,
    [Status] NVARCHAR(MAX) NOT NULL,
    [CreatedBy] NVARCHAR(100) NOT NULL,
    [CreatedDate] DATETIME2 NOT NULL,
    [LastModified] DATETIME2 NOT NULL,
    CONSTRAINT [PK_Bookings] PRIMARY KEY ([BookingId]),
    CONSTRAINT [FK_Bookings_Clients_ClientId] FOREIGN KEY ([ClientId]) REFERENCES [Clients]([ClientId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Bookings_Events_EventId] FOREIGN KEY ([EventId]) REFERENCES [Events]([EventId]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Bookings_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues]([VenueId]) ON DELETE NO ACTION
);

-- Create Indexes
CREATE INDEX [IX_Bookings_ClientId] ON [Bookings]([ClientId]);
CREATE UNIQUE INDEX [IX_Bookings_EventId] ON [Bookings]([EventId]);
CREATE INDEX [IX_Bookings_VenueId] ON [Bookings]([VenueId]);
CREATE INDEX [IX_Events_VenueId] ON [Events]([VenueId]);
CREATE INDEX [IX_Events_EventTypeId] ON [Events]([EventTypeId]);

-- Insert default event types
INSERT INTO [EventTypes] ([Name], [Description], [IsActive]) VALUES
('Conference', 'Professional gatherings, seminars, and business meetings', 1),
('Wedding', 'Wedding ceremonies and receptions', 1),
('Concert', 'Musical performances and live entertainment events', 1),
('Exhibition', 'Art exhibitions, trade shows, and product launches', 1),
('Corporate Event', 'Team building, workshops, and company celebrations', 1),
('Social Gathering', 'Birthday parties, anniversaries, and social celebrations', 1),
('Sports Event', 'Sports competitions and athletic events', 1),
('Workshop', 'Educational and training sessions', 1),
('Charity Event', 'Fundraising and charitable gatherings', 1),
('Private Party', 'Private celebrations and exclusive gatherings', 1);
