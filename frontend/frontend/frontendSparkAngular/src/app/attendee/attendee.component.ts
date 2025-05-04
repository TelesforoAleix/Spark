import { Component, Input } from '@angular/core';
import { Attendee } from '../model/attendee';
import { AttendeeService } from '../services/attendee.service';
import { Router } from '@angular/router';
import { NgForOf } from '@angular/common';

@Component({
  selector: 'app-attendee',
  standalone: true,
  imports: [NgForOf],
  templateUrl: './attendee.component.html',
  styleUrl: './attendee.component.css'
})

export class AttendeeComponent {
  @Input() attendee!: Attendee;

  constructor(private attendeeService: AttendeeService, private router : Router) {}

  deleteAttendee(): void {
    if (this.attendee?.attendeeId) {
      this.attendeeService.deleteAttendee(this.attendee.attendeeId).subscribe();
    } else {
      console.error('Attendee ID is undefined');
    }
    }

  editAttendee(attendeeId: string): void {
    if (attendeeId) {
      this.router.navigate(['/edit-attendee', attendeeId]);
    } else {
      console.error('Attendee ID is undefined');
    }
}
}
