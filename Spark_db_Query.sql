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
