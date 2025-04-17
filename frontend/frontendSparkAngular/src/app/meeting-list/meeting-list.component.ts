// Component: Displays a table of all scheduled meetings
// Fetches meetings using MeetingService (HTTP GET)

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
  meetings: Meeting[] = [];

  constructor(private meetingService: MeetingService) {}

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
