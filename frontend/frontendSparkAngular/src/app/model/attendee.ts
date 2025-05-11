export interface Attendee {
    attendeeId: string;   // varchar(8)
    eventId: number | null; // varchar(8) or null
    firstName: string;     // varchar(20)
    lastName: string;      // varchar(20)
    email: string;          // varchar(50)
    hashed_password?: string; // varchar(256)
    header: string;         // varchar(50)
    bio: string;            // varchar(400)
    link: string;           // varchar(255)
  }
  