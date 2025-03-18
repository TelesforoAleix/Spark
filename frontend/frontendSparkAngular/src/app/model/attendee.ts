export interface Attendee {
    attendee_id: string;   // varchar(8)
    event_id: string | null; // varchar(8) or null
    first_name: string;     // varchar(20)
    last_name: string;      // varchar(20)
    email: string;          // varchar(50)
    hashed_password: string; // varchar(256)
    header: string;         // varchar(50)
    bio: string;            // varchar(400)
    link: string;           // varchar(255)
  }
  