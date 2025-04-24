// Purpose: Displays a table of all scheduled meetings fetched from the backend.
// Notes: Uses the MeetingService to perform an HTTP GET request.
//        Automatically runs the request on component initialization via ngOnInit().

import { Component, OnInit } from '@angular/core';
import { MeetingService } from '../services/meeting.service';
import { Meeting } from '../model/meeting.model';

@Component({
  selector: 'app-meeting-list',
  standalone: true,
  templateUrl: './meeting-list.component.html',
  styleUrls: ['./meeting-list.component.css'],
  imports: []
})
export class MeetingListComponent implements OnInit {
  // Stores the list of meetings retrieved from the API
  meetings: Meeting[] = [];

  // Injects the MeetingService for making HTTP requests
  constructor(private meetingService: MeetingService) {}

  // Runs on component load to fetch all meetings from backend
  ngOnInit(): void {
    this.meetingService.getAllMeetings().subscribe({
      next: (data) => {
        this.meetings = data;
      },
      error: (err) => {
        console.error('❌ Error loading meetings', err);
      }
    });
  }
}
