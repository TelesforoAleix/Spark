import { Component, OnInit } from '@angular/core';
import { Attendee } from '../model/attendee';
import { AttendeeComponent } from '../attendee/attendee.component';
import { AttendeeService } from '../services/attendee.service';

@Component({
  selector: 'app-attendee-list',
  standalone: true,
  imports: [AttendeeComponent],
  templateUrl: './attendee-list.component.html',
  styleUrl: './attendee-list.component.css'
})

export class AttendeeListComponent implements OnInit {

  constructor(private attendeeService: AttendeeService) { }

  attendees: Attendee[] = [];

  ngOnInit(): void {
    this.attendeeService.getAttendees().subscribe(
      (data) => {
        console.log('Fetched attendees:', data);
        this.attendees = data;
      },
      (error) => {
        console.error('Error fetching attendees:', error);
      }
    );
  }
}
