# CLOUD DEVELOPMENT A (CLDV6211)

## Portfolio of Evidence (POE) Assignment

**Total Mark Allocation:** 300 Marks  
**Suggested Completion Time:** Minimum of 45 hours

---

## Background

EventEase is an up-and-coming event management company that coordinates various events across different venues. Due to rapid growth and increasing demand, they've been overwhelmed with managing multiple venue bookings, scheduling, and handling booking conflicts.

Currently, all scheduling and bookings are managed manually, leading to:

- Double bookings
- Last-minute cancellations
- Lack of visibility into venue availability

EventEase has contracted your development team to build an efficient, user-friendly online booking platform to streamline these processes. The application should:

- Simplify venue management
- Prevent booking conflicts
- Provide a clear view of scheduled events
- Enable guests to book venues based on availability
- Offer search and filtering capabilities for easy management

**Important Note:** The first phase focuses on building the admin platform. Customers will not make bookings themselves but can request bookings via email, call, or in person. A booking specialist will use the system to make the booking for them. Events can be loaded into the system before a venue becomes available.

### CEO Requirements

- User-friendly web application for booking specialists to browse venues, view events, and manage bookings
- System allowing authorized personnel to create, view, update, and delete venue and event information
- Secure handling of image uploads for each venue
- Prevention of double bookings and restrictions on deleting venues/events with existing bookings
- Secure cloud storage of all booking information with backups
- Storage of important details including locations, capacity, venue/event names, start & end dates, and unique booking IDs

EventEase plans to integrate this platform with additional cloud services as their needs grow.

---

## Introduction

You will build a Venue Booking System for EventEase using ASP.NET Core MVC, deployed in stages to the cloud.

The application will follow three main development phases, each building upon the previous phase, to progressively introduce core functionalities and cloud services.

### Development Phases Overview

#### Part 1: Project Foundation and Initial Deployment

1. Database Design
2. Initial Application Development
3. Deploy Initial Version to Azure

#### Part 2: Enhancing Functionality and Integrating Cloud Storage

1. Error Handling and Validation
2. Image Management with Azure Blob Storage
3. Enhanced Display and Search
4. Azure Deployment Updates

#### Part 3: Advanced Filtering, Reporting, and Documentation

1. Filtering and Event Type Integration
2. Final Deployment and Reflective Report

By the end of this project, EventEase should have a fully functional Venue Booking System with essential features, from event, venue, and booking management to cloud-stored images and filtering capabilities.

---

## Instructions

For this Portfolio of Evidence (POE), you will create the Web Application, Database Storage, Azure Compute, and Azure Data Storage components for EventEase, progressively developed through Parts 1, 2, and culminating in the final POE solution.

### Requirements

- Access to an Azure account with available credit (arranged by your lecturer/campus)
- Microsoft Visual Studio for coding
- GitHub repository access (arranged by your lecturer/campus)

### Submission Guidelines

For each part of the POE, create a document in MS-Word containing:

- Your ST number
- The module code
- URL to your GitHub repository source code
- All relevant URLs required (especially the URL of the deployed Web App)
- All required answers, including typed answers, diagrams, and screenshots

**Document naming format:** StudentNumber_ModuleCode_Part#  
(e.g., 12345_CLDV6211_Part1)

---

## Part 1 — Project Foundation and Initial Deployment (100 Marks)

**Related Content:** Learning Units 1-2

### Learning Outcomes

**Practical Skills:**

- Design and create a Basic Database Structure
- Implement ASP.NET Core MVC Basics
- Integrate Azure SQL Database with ASP.NET Core
- Deploy an ASP.NET Core web application to Azure

**Theoretical Knowledge:**

- Explain the difference between deploying on-premises and in the cloud
- Identify key differences between Azure hosting models

### Tasks

#### A. Design the EventEase Database

Based on the background information, develop an Entity-Relationship Diagram with an accompanying Database Script. Your database should contain three main tables:

1. **Venue:** Contains VenueId, VenueName, Location, Capacity, and ImageUrl
2. **Event:** Contains EventId, EventName, EventDate, Description, and VenueId
3. **Booking:** An associative table linking Venue and Event (BookingId, EventId, VenueId, BookingDate)

You may add additional tables but keep the structure simple.

#### B. Develop the Web Application

Build an ASP.NET Core web application with corresponding models, controllers, and views including:

- Relevant Models, Controllers, and Views for Venues, Events & Bookings
- CRUD functionality with database communication
- Placeholder URLs for venue and event images

#### C. Deploy your Web App and Database to Microsoft Azure

- Set up an Azure web app service and deploy your web app
- Set up an Azure SQL Database and migrate your local data

#### D. Cloud Computing Basics (Theory Questions)

1. In what ways does deploying an application in the cloud differ from deploying it on-premises, particularly regarding security, deployment speed, and resource management? Use examples to illustrate your points.
    
2. What are the key differences between Infrastructure as a Service (IaaS), Platform as a Service (PaaS), and Software as a Service (SaaS), and why might EventEase benefit from the use of PaaS over the other two when building a new application? Use examples to support your answer.
    

### Submission Notes

- Include all relevant materials (web app, ERD, Database Script, Word document, etc.)
- Provide all relevant URLs (web app URL, etc.)
- Include code attribution & traditional referencing

---

## Part 2 — Enhancing Functionality and Integrating Cloud Storage (100 Marks)

**Related Content:** Learning Units 3-6

### Learning Outcomes

**Practical Skills:**

- Implement Error Handling and Validation
- Manage Images with Azure Blob Storage
- Enhance Application Interface and Display
- Implement Basic Search Functionality
- Publish Updated Application and Services to Azure

**Theoretical Knowledge:**

- Explain how Azure cognitive search differs from traditional search engines
- Discuss the importance of normalization in cloud-based database design
- Compare differences between relational and NoSQL cloud-based databases
- Discuss how Durable functions can benefit a cloud application's performance

### Tasks

#### A. Integrate Azure Storage

Replace placeholder URLs with image upload functionality, storing images securely in Azure Blob Storage for Venues and Events.

#### B. Implementing Error Handling and Validation

- Add validation to prevent double booking of venues on the same date/time
- Add validation to restrict deletion of venues/events with active bookings
- Ensure the application doesn't crash on common user errors
- Display alerts for validation errors

#### C. Enhanced Display and Search

- Create a consolidated view with relevant booking information from venue and event tables
- Add a simple search function to find bookings via BookingID or Event Name

#### D. Azure Deployment Updates

- Deploy the updated web application with Azure storage and search integration
- Update your database to include the newly created view

#### E. Database Design, Cognitive Search (Theory Questions)

1. Explain how Azure's Cognitive Search differs from traditional search engines and discuss potential use cases where Cognitive Search would offer a clear advantage. What limitations does it have, and how can they be mitigated?
    
2. Why is database normalization important in cloud-based database design? Discuss the impact of both normalized and denormalized structures on performance and scalability in a cloud environment like Azure.
    

### Submission Notes

- Include all relevant materials
- Provide all relevant URLs (web app URL, etc.)
- Include code attribution & traditional referencing

---

## Part 3 — Advanced Filtering, Reporting, and Documentation (100 Marks)

**Related Content:** Learning Units 7-9

### Learning Outcomes

**Practical Skills:**

- Enhance Search with Filtering and Event Type Classification
- Finalize Application Deployment and Documentation
- Reflect on Project and Cloud Skills Developed

**Theoretical Knowledge:**

- Discuss how Cosmos DB differs from traditional databases
- Discuss key considerations when designing logic apps that handle sensitive data
- Explain how combining Event Grid with other services can create robust workflows

### Tasks

#### A. Advanced Filtering

- Add a new EventType lookup table with predefined categories and an availability field to your venue table
- Implement filters to filter results by event type, date range, and venue availability

#### B. Azure Deployment Updates

- Deploy the updated web application with filter feature integration
- Update your database to include the newly created table and field

#### C. Reflective Technical Report

Write a report documenting your development journey, including:

1. **Detailed description of the application's full feature list**
    
2. **Component discussion**
    
    - Discuss which Azure services were used for each part of the project and why
    - Include potential alternatives that might have been more suitable
    - Discuss technologies used to build the project and their selection rationale
3. **Project reflection**
    
    - Reflect on the entire project from Part 1 to completion
    - Discuss what you've learned about designing, developing, and deploying cloud-based applications

### Submission Notes

- Include all relevant materials
- Provide all relevant URLs (web app URL, etc.)
- Include code attribution & traditional referencing

---

## Assessment Criteria Summary

### Part 1 (100 marks)

- Database Design (30 marks)
- Web Application Development (30 marks)
- Azure Deployment (20 marks)
- Cloud Computing Theory (20 marks)

### Part 2 (100 marks)

- Azure Storage Integration (20 marks)
- Error Handling & Validation (20 marks)
- Enhanced Display & Search (20 marks)
- Azure Deployment Updates (20 marks)
- Database Design Theory (20 marks)

### Part 3 (100 marks)

- Advanced Filtering (20 marks)
- Azure Deployment Updates (20 marks)
- Technical Report - Feature List (20 marks)
- Technical Report - Component Discussion (20 marks)
- Technical Report - Project Reflection (20 marks)

**TOTAL: 300 MARKS**