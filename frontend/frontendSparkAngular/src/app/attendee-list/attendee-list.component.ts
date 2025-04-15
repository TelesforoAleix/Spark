import { Component, OnInit, Input } from '@angular/core';
import { Attendee } from '../model/attendee';
import { AttendeeComponent } from '../attendee/attendee.component';
import { AttendeeService } from '../services/attendee.service';
import { NgForOf } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-attendee-list',
  standalone: true,
  imports: [AttendeeComponent, NgForOf],
  templateUrl: './attendee-list.component.html',
  styleUrl: './attendee-list.component.css'
})

export class AttendeeListComponent implements OnInit {

  constructor(private attendeeService: AttendeeService, private router : Router) { }
  
  @Input() attendee!: Attendee;

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
  
    deleteAttendee(attendeeId: string): void {
      if (attendeeId) {
        this.attendeeService.deleteAttendee(attendeeId).subscribe();
        console.log('Attendee deleted successfully');
        this.reloadPage();
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

  reloadPage(): void {
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate(['/attendee']);
    });
  }
}
