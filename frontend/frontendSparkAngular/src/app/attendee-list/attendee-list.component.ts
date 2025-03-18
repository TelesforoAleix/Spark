import { Component } from '@angular/core';
import { Attendee } from '../model/attendee';
import { AttendeeComponent } from '../attendee/attendee.component';

@Component({
  selector: 'app-attendee-list',
  standalone: true,
  imports: [AttendeeComponent],
  templateUrl: './attendee-list.component.html',
  styleUrl: './attendee-list.component.css'
})

export class AttendeeListComponent {

   attendees: Attendee[] = [
    {
      attendee_id: 'A0000001',
      event_id: 'E001',
      first_name: 'John',
      last_name: 'Doe',
      email: 'john.doe@example.com',
      hashed_password: 'hashedpassword1',
      header: 'Software Engineer',
      bio: 'A passionate software engineer with a love for coding.',
      link: 'https://johnslink.com'
    },
    {
      attendee_id: 'A0000002',
      event_id: 'E002',
      first_name: 'Jane',
      last_name: 'Smith',
      email: 'jane.smith@example.com',
      hashed_password: 'hashedpassword2',
      header: 'Data Scientist',
      bio: 'Specialized in data analytics and machine learning.',
      link: 'https://janeslink.com'
    },
    {
      attendee_id: 'A0000003',
      event_id: 'E001',
      first_name: 'Alex',
      last_name: 'Johnson',
      email: 'alex.johnson@example.com',
      hashed_password: 'hashedpassword3',
      header: 'Product Manager',
      bio: 'Focused on creating impactful products with a user-first mindset.',
      link: 'https://alexlink.com'
    },
    {
      attendee_id: 'A0000004',
      event_id: 'E003',
      first_name: 'Emily',
      last_name: 'Brown',
      email: 'emily.brown@example.com',
      hashed_password: 'hashedpassword4',
      header: 'UX Designer',
      bio: 'A UX designer who loves crafting intuitive user experiences.',
      link: 'https://emilylink.com'
    },
    {
      attendee_id: 'A0000005',
      event_id: 'E001',
      first_name: 'Michael',
      last_name: 'Davis',
      email: 'michael.davis@example.com',
      hashed_password: 'hashedpassword5',
      header: 'DevOps Engineer',
      bio: 'Experienced in building scalable infrastructure and automation.',
      link: 'https://michaellink.com'
    },
    {
      attendee_id: 'A0000006',
      event_id: 'E002',
      first_name: 'Sophia',
      last_name: 'Miller',
      email: 'sophia.miller@example.com',
      hashed_password: 'hashedpassword6',
      header: 'Frontend Developer',
      bio: 'Frontend developer passionate about creating interactive UIs.',
      link: 'https://sophialink.com'
    },
    {
      attendee_id: 'A0000007',
      event_id: 'E004',
      first_name: 'William',
      last_name: 'Martinez',
      email: 'william.martinez@example.com',
      hashed_password: 'hashedpassword7',
      header: 'Cybersecurity Expert',
      bio: 'Specialist in protecting systems and data from security threats.',
      link: 'https://williamlink.com'
    },
    {
      attendee_id: 'A0000008',
      event_id: 'E003',
      first_name: 'Olivia',
      last_name: 'Garcia',
      email: 'olivia.garcia@example.com',
      hashed_password: 'hashedpassword8',
      header: 'Marketing Specialist',
      bio: 'Experienced in digital marketing and content strategy.',
      link: 'https://olivialink.com'
    },
    {
      attendee_id: 'A0000009',
      event_id: 'E004',
      first_name: 'James',
      last_name: 'Rodriguez',
      email: 'james.rodriguez@example.com',
      hashed_password: 'hashedpassword9',
      header: 'Backend Developer',
      bio: 'Backend developer focusing on scalable web applications.',
      link: 'https://jameslink.com'
    },
    {
      attendee_id: 'A0000010',
      event_id: 'E002',
      first_name: 'Ava',
      last_name: 'Hernandez',
      email: 'ava.hernandez@example.com',
      hashed_password: 'hashedpassword10',
      header: 'Cloud Engineer',
      bio: 'Cloud solutions architect with a focus on AWS and Azure.',
      link: 'https://avalink.com'
    }
  ];
  
}
