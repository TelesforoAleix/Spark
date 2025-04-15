-- Create the 'event' table
CREATE TABLE event (
    event_id SERIAL PRIMARY KEY, -- Auto-incrementing ID
    name TEXT NOT NULL, -- Event name
    startdate DATE NOT NULL, -- Start date of the event
    finishdate DATE NOT NULL, -- Finish date of the event
    location TEXT NOT NULL, -- Location of the event
    bio TEXT, -- Short bio or description of the event
    numberofmaxattendee INTEGER NOT NULL, -- Maximum number of attendees
    numberoftables INTEGER NOT NULL -- Number of tables for the event
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

-- Script to add test data to the database

-- Insert test data into the 'event' table
INSERT INTO event (name, startdate, finishdate, location, bio, numberofmaxattendee, numberoftables)
VALUES
('Tech Networking Summit', '2025-03-20', '2025-03-20', 'Copenhagen Business Center', 'Bringing tech professionals together', 100, 10);

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

/*

/////// LEGACY DATABASE SCHEMA ///////
-- The following SQL code is a legacy schema for the database. It is commented out and not used in the current implementation.

create table organizer
	(org_id varchar(8),
	name varchar(20),
	email varchar(50),
	hash_password varchar(256),
	primary key (org_id)
);

create table event
	(event_id varchar(8),
	org_id varchar(8) null,
	start_date timestamp,
	end_date timestamp,
	location varchar(255),
	event_bio varchar(400),
	primary key (event_id),
	foreign key (org_id) references organizer (org_id) on delete set null
);

 create table session
	(session_id varchar(8),
	event_id varchar(8) null,
	start_time time,
	session_duration int,
	num_tables int,
	appt_duration int,
	break_duration int,
	location varchar(255),
	primary key (session_id),
	foreign key (event_id) references event (event_id) on delete set null
);

create table desk
	(desk_id varchar(8),
	session_id varchar(8) null,
	start_time time,
	finish_time time,
	desk_ref int,
	primary key (desk_id),
	foreign key (session_id) references session (session_id) on delete set null
);

create table attendee
	(attendee_id varchar(8),
	event_id varchar(8) null,
	first_name varchar(20),
	last_name varchar(20),
	email varchar(50),
	hashed_password varchar(256),
	header varchar(50),
	bio varchar(400),
	link varchar(255),
	time_availability json,
	preferences json, 
	primary key (attendee_id),
	foreign key (event_id) references event (event_id) on delete set null
);

 
create table appointment
	(appt_id varchar(8),
	attendee1_id varchar(8) null,
	attendee2_id varchar(8) null,
	meeting_desk varchar(8) null,
	primary key (appt_id),
	foreign key (attendee1_id) references attendee (attendee_id) on delete set null,
	foreign key (attendee2_id) references attendee (attendee_id) on delete set null,
	foreign key (meeting_desk) references desk (desk_id) on delete set null
);

*/

