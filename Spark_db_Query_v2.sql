-- Create the user table for authentication
CREATE TABLE "user" (
    id SERIAL PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL
);

-- Create the 'event' table
CREATE TABLE event (
    event_id SERIAL PRIMARY KEY, -- Auto-incrementing ID
    name TEXT NOT NULL, -- Event name
    startdate timestamp, -- Start date of the event
    finishdate timestamp, -- Finish date of the event
    location TEXT, -- Location of the event
    bio TEXT, -- Short bio or description of the event
    networking_startdate timestamp, -- Start date of networking
	networking_finishdate timestamp, -- End date of networking
	meeting_duration INTEGER, -- Time duration of each meeting
	break_duration INTEGER, -- Time duration of breaks between meetings
	available_tables INTEGER -- Number of available tables
);

-- Create the 'attendee' table
CREATE TABLE attendee (
    attendee_id VARCHAR(8) UNIQUE NOT NULL, -- External identifier for the attendee
    event_id INTEGER REFERENCES event(event_id) ON DELETE CASCADE, -- Foreign key to the event table
    first_name VARCHAR(20) NOT NULL, -- First name of the attendee
    last_name VARCHAR(20) NOT NULL, -- Last name of the attendee
    email VARCHAR(50) NOT NULL, -- Email of the attendee
    hashed_password VARCHAR(256) NOT NULL, -- Hashed password for the attendee
    header VARCHAR(50), -- Header or title for the attendee
    bio VARCHAR(400), -- Short bio of the attendee
    link VARCHAR(255) -- Link to the attendee's profile or website
);

-- Create the 'appointment' table for meetings
CREATE TABLE appointment (
    id SERIAL PRIMARY KEY, -- Auto-incrementing ID
    event_id INTEGER REFERENCES event(event_id) ON DELETE CASCADE, -- Foreign key to the event table
    attendee1_id VARCHAR(8) REFERENCES attendee(attendee_id) ON DELETE CASCADE, -- First attendee
    attendee2_id VARCHAR(8) REFERENCES attendee(attendee_id) ON DELETE CASCADE, -- Second attendee
    table_name VARCHAR(20) NOT NULL, -- Name of the table/desk
    date DATE NOT NULL, -- Date of the meeting
    start_time TIMESTAMP NOT NULL, -- Start time of the meeting
    finish_time TIMESTAMP NOT NULL -- End time of the meeting
);

-- Insert sample users
INSERT INTO "user" (username, password)
VALUES 
('admin', 'admin123'),
('manager', 'manager123'),
('organizer', 'organizer123');

-- Insert test data into the 'event' table (without numberofmaxattendee and numberoftables)
INSERT INTO event (name, startdate, finishdate, location, bio, networking_startdate, networking_finishdate, meeting_duration, break_duration, available_tables)
VALUES
('Tech Networking Summit', '"2025-03-16 10:00:00"', '"2025-03-16 17:00:00"', 'Copenhagen Business Center', 'Bringing tech professionals together', '"2025-03-16 11:00:00"', '"2025-03-16 16:00:00"', 10, 5, 10);

-- Insert test data into the 'attendee' table
INSERT INTO attendee (attendee_id, event_id, first_name, last_name, email, hashed_password, header, bio, link)
VALUES
('AT0001', 1, 'Alice', 'Johnson', 'alice@tech.com', 'hashed_pw_1', 'AI Researcher', 'Passionate about AI and ML innovations.', 'https://linkedin.com/alice'),
('AT0002', 1, 'Bob', 'Smith', 'bob@startups.com', 'hashed_pw_2', 'Startup Founder', 'Founder of an early-stage fintech startup.', 'https://linkedin.com/bob'),
('AT0003', 1, 'Charlie', 'Lee', 'charlie@green.com', 'hashed_pw_3', 'Climate Scientist', 'Focused on sustainable energy and carbon reduction.', 'https://linkedin.com/charlie'),
('AT0004', 1, 'Diana', 'Prince', 'diana@wonder.com', 'hashed_pw_4', 'Tech Advocate', 'Advocating for women in technology.', 'https://linkedin.com/diana'),
('AT0005', 1, 'Eve', 'Adams', 'eve@cyber.com', 'hashed_pw_5', 'Cybersecurity Expert', 'Specialist in ethical hacking and security.', 'https://linkedin.com/eve'),
('AT0006', 1, 'Frank', 'Wright', 'frank@iot.com', 'hashed_pw_6', 'IoT Engineer', 'Building smart devices for the future.', 'https://linkedin.com/frank'),
('AT0007', 1, 'Grace', 'Hopper', 'grace@code.com', 'hashed_pw_7', 'Software Engineer', 'Pioneer in computer programming.', 'https://linkedin.com/grace'),
('AT0008', 1, 'Hank', 'Pym', 'hank@quantum.com', 'hashed_pw_8', 'Quantum Scientist', 'Exploring quantum computing.', 'https://linkedin.com/hank'),
('AT0009', 1, 'Ivy', 'Green', 'ivy@eco.com', 'hashed_pw_9', 'Environmental Scientist', 'Working on sustainable solutions.', 'https://linkedin.com/ivy'),
('AT0010', 1, 'Jack', 'Ryan', 'jack@ai.com', 'hashed_pw_10', 'AI Specialist', 'Developing AI-driven applications.', 'https://linkedin.com/jack'),
('AT0011', 1, 'Karen', 'White', 'karen@ml.com', 'hashed_pw_11', 'Machine Learning Engineer', 'Building predictive models.', 'https://linkedin.com/karen'),
('AT0012', 1, 'Leo', 'Brown', 'leo@robotics.com', 'hashed_pw_12', 'Robotics Engineer', 'Designing autonomous robots.', 'https://linkedin.com/leo'),
('AT0013', 1, 'Mia', 'Clark', 'mia@design.com', 'hashed_pw_13', 'UI/UX Designer', 'Creating user-friendly interfaces.', 'https://linkedin.com/mia'),
('AT0014', 1, 'Nina', 'Davis', 'nina@data.com', 'hashed_pw_14', 'Data Scientist', 'Analyzing big data for insights.', 'https://linkedin.com/nina'),
('AT0015', 1, 'Oscar', 'Evans', 'oscar@blockchain.com', 'hashed_pw_15', 'Blockchain Developer', 'Building decentralized applications.', 'https://linkedin.com/oscar');

