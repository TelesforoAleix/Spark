import { Component, OnInit, Input } from '@angular/core';
import { Meeting } from '../model/meeting';
import { MeetingComponent } from '../meeting/meeting.component';
import { MeetingService } from '../services/meeting.service';
import { NgForOf, DatePipe } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-meeting-list',
  standalone: true,
  imports: [MeetingComponent, NgForOf],
  providers: [DatePipe],
  templateUrl: './meeting-list.component.html',
  styleUrl: './meeting-list.component.css'
})

export class MeetingListComponent implements OnInit {

  constructor(private meetingService: MeetingService, private router: Router, private datePipe: DatePipe) { }
  
  @Input() meeting!: Meeting;

  meetings: Meeting[] = [];

  ngOnInit(): void {
    this.meetingService.getMeetings().subscribe(
      (data) => {
        console.log('Fetched meetings:', data);
        this.meetings = data;
      },
      (error) => {
        console.error('Error fetching meetings:', error);
      }
    );
  }
  
    deleteMeeting(meetingId: string): void {
      if (meetingId) {
        this.meetingService.deleteMeeting(meetingId).subscribe();
        console.log('Meeting deleted successfully');
        this.reloadPage();
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

  reloadPage(): void {
    this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
      this.router.navigate(['/meeting']);
    });
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
