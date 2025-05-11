# Spark - Networking Event Management System

## Overview
Spark is a full-stack web application designed for managing networking/speed-dating events. It enables event organizers to create events, manage attendees, and automatically generate meeting schedules that optimize networking opportunities.

## Technologies Used

### Frontend
- Angular 16+
- TypeScript
- HTML/CSS

### Backend
- ASP.NET Core Web API
- C#
- PostgreSQL database

## Features

### Event Management
- View and edit event details
- Set networking periods, meeting durations, and break times
- Configure available tables for meetings

### Attendee Management
- Add, view, edit, and delete event attendees
- Store attendee profile information (name, title, bio, contact details)
- Search and filter attendees

### Meeting Scheduling
- Automatically generate optimal meeting schedules
- View schedules by table or time slot
- Ensure each attendee meets with as many others as possible

### Authentication
- Secure login system with Basic Authentication
- Protected API endpoints

## Setup Instructions

### Prerequisites
- Node.js and npm
- .NET 7.0 SDK
- PostgreSQL database

### Database Setup
1. Install PostgreSQL if not already installed
2. Create a new database named `spark_db`
3. Run the SQL script in `Spark_db_Query_v2.sql` to create tables and sample data

### Backend Setup
1. Navigate to the backend folder:
   ```
   cd backend
   ```
2. Restore NuGet packages:
   ```
   dotnet restore
   ```
3. Update the database connection string in `backendSpark.API/appsettings.json` to match your PostgreSQL configuration
4. Run the API:
   ```
   cd backendSpark.API
   dotnet run
   ```
   The API will be available at `http://localhost:5193`

### Frontend Setup
1. Navigate to the frontend folder:
   ```
   cd frontend/frontendSparkAngular
   ```
2. Install dependencies:
   ```
   npm install
   ```
3. Start the Angular development server:
   ```
   ng serve
   ```
4. Open a browser and navigate to `http://localhost:4200`

## Default Login Credentials
- Username: `admin`
- Password: `admin123`

## Project Structure

### Backend Structure
```
backend/
├── backendSpark.API/        # API controllers and configuration
│   ├── Controllers/         # API endpoints
│   ├── Middleware/          # Authentication middleware
│   └── Program.cs           # Application setup
├── backendSpark.Model/      # Data models and repositories
│   ├── Entities/            # Domain objects
│   └── Repositories/        # Data access layer
```

### Frontend Structure
```
frontend/frontendSparkAngular/
├── src/
│   ├── app/
│   │   ├── add-attendee/    # Add attendee component
│   │   ├── attendee-list/   # Attendee listing component
│   │   ├── edit-attendee/   # Edit attendee component
│   │   ├── edit-event/      # Edit event component
│   │   ├── event-info/      # Event details component
│   │   ├── login/           # Authentication component
│   │   ├── meeting-list/    # Meeting schedule component
│   │   ├── model/           # TypeScript interfaces
│   │   ├── services/        # API communication services
│   │   ├── app.component.*  # Main application component
│   │   └── app.routes.ts    # Routing configuration
│   └── assets/              # Static assets
```

## API Endpoints

### Authentication
- `POST /api/Login` - Authenticate user

### Event Management
- `GET /api/Event` - Get event details
- `PUT /api/Event` - Update event information

### Attendee Management
- `GET /api/Attendee` - Get all attendees
- `GET /api/Attendee/{id}` - Get attendee by ID
- `POST /api/Attendee` - Create new attendee
- `PUT /api/Attendee/{id}` - Update existing attendee
- `DELETE /api/Attendee/{id}` - Delete attendee

### Meeting Management
- `GET /api/Meeting` - Get all meetings
- `POST /api/Meeting/GenerateSchedule` - Generate meeting schedule

## Usage Guide

1. **Login** using the default credentials
2. **Configure Event Details** including date, location, networking session times, meeting duration
3. **Manage Attendees** by adding participant information
4. **Generate Meeting Schedule** to automatically create an optimal networking plan
5. **View Meetings** in either Table or Time view to see who meets with whom and when

## Algorithm Features

The meeting schedule generator algorithm:
- Creates meetings based on available time slots and tables
- Ensures attendees don't have overlapping meetings
- Maximizes the number of unique connections
- Balances meeting distribution among participants
- Provides a fair networking opportunity for all attendees

## Notes for Developers

- The application is designed with a focus on maintainability and separation of concerns
- The repository pattern is used for data access
- Authentication is handled via middleware
- Frontend components communicate with the backend via services
- The project includes basic test structures that can be expanded

## License
This project is created for educational purposes as part of the Copenhagen Business School Applied Programming course.