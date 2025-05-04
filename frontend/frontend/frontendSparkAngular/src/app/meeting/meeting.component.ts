import { Component, Input, ViewEncapsulation } from '@angular/core';
import { Meeting } from '../model/meeting';
import { MeetingService } from '../services/meeting.service';
import { Router } from '@angular/router';
import { DatePipe, NgForOf } from '@angular/common';

@Component({
  selector: 'app-meeting',
  standalone: true,
  imports: [NgForOf],
  providers: [DatePipe],
  templateUrl: './meeting.component.html',
  styleUrl: './meeting.component.css',
  encapsulation: ViewEncapsulation.None
})

export class MeetingComponent {
  @Input() meeting!: Meeting;

  constructor(private meetingService: MeetingService, private router : Router, private datePipe: DatePipe) {}

  deleteMeeting(): void {
    if (this.meeting?.meetingId) {
      this.meetingService.deleteMeeting(this.meeting.meetingId).subscribe();
    } else {
      console.error('Meeting ID is undefined');
    }
    }

  editMeeting(meetingId: string): void {
    if (meetingId) {
      this.router.navigate(['/edit-meeting', meetingId]);
    } else {
      console.error('Meeting ID is undefined');
    }
  }

  formatTime(dateTime: Date | string | null | undefined): string {
    if (!dateTime) {
      return ''; // Return empty string for null or undefined values
    }
    
    // If dateTime is already a string, make sure it's parsed to a Date first
    if (typeof dateTime === 'string') {
      return this.datePipe.transform(new Date(dateTime), 'shortTime') || '';
    }
    
    return this.datePipe.transform(dateTime, 'shortTime') || '';
  }
}

