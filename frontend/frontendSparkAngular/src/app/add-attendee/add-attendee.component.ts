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
    attendeeId: 'AT' + Math.floor(Math.random() * 9999), 
    eventId: 1,     // Default value for eventId
    hashed_password: '', // Default value for hashed_password
    firstName: '',
    lastName: '',
    email: '',
    header: '',
    bio: '',
    link: ''
  };

  constructor(
    private attendeeService: AttendeeService,
    private router: Router,
  ) {}

  ngOnInit() {
    // Nothing to initialize for add form
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