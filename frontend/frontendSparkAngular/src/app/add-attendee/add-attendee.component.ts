import { Component, OnInit } from '@angular/core';
import { AttendeeService } from '../services/attendee.service';
import { Attendee } from '../model/attendee';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-attendee',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './add-attendee.component.html',
  styleUrl: './add-attendee.component.css'
})
export class AddAttendeeComponent implements OnInit {
  // Initialize with empty properties
  newAttendee: Attendee = {
    attendeeId: this.generateAttendeeId(),
    eventId: 1,     // Default value for eventId
    hashed_password: 'password123', // Default value for hashed_password
    firstName: '',
    lastName: '',
    email: '',
    header: '',
    bio: '',
    link: ''
  };

    // Generate a valid attendee ID in format "ATXXXX"
    private generateAttendeeId(): string {
      // Generate a random 4-digit number (1000-9999)
      const randomDigits = 1000 + Math.floor(Math.random() * 9000);
      return `AT${randomDigits}`;
    }

  constructor(
    private attendeeService: AttendeeService,
    private router: Router,
  ) {}

  ngOnInit() {
    if (this.attendeeService.authHeader == null) { 
      this.router.navigate(["login"]); 
      return; 
  }
  }

  saveAttendee(): void {
    // Validate required fields
    if (!this.newAttendee.firstName || !this.newAttendee.lastName || !this.newAttendee.email) {
      alert('Please fill in all required fields');
      return;
    }

    // Call service to create new attendee
    this.attendeeService.createAttendee(this.newAttendee).subscribe(
      (createdAttendee) => {
        console.log('Attendee created successfully:', createdAttendee);
        this.router.navigate(['/attendee']);
      },
      (error) => {
        console.error('Error creating attendee:', error);
        console.error(this.newAttendee);
      }
    );
  }

  cancelAdd(): void {
    this.router.navigate(['/attendee']);
  }
}