import { Component, OnInit } from '@angular/core';
import { Meeting } from '../model/meeting';
import { MeetingService } from '../services/meeting.service';
import { Router } from '@angular/router';
import { DatePipe, NgForOf, NgIf, NgClass, CommonModule } from '@angular/common';
import { Observable, tap } from 'rxjs';

@Component({
  selector: 'app-meeting-list',
  standalone: true,
  imports: [NgForOf, NgIf, NgClass, CommonModule],
  providers: [DatePipe],
  templateUrl: './meeting-list.component.html',
  styleUrl: './meeting-list.component.css'
})
export class MeetingListComponent implements OnInit {
  constructor(private meetingService: MeetingService, private router: Router, private datePipe: DatePipe) { }
  
  meetings: Meeting[] = [];
  timeSlots: string[] = [];
  tables: string[] = [];
  meetingsByTableAndTime: { [table: string]: { [timeSlot: string]: Meeting } } = {};
  meetingsByTimeAndTable: { [timeSlot: string]: { [table: string]: Meeting } } = {};
  
  viewMode: 'time' | 'table' = 'time';
  
  // For delete confirmation
  showDeleteModal: boolean = false;
  selectedMeeting: Meeting | null = null;

  ngOnInit(): void {
    if (this.meetingService.authHeader == null) { 
      this.router.navigate(["login"]); 
      return; 
  }
    this.loadMeetings();
  }
  
  loadMeetings(): void {
    this.meetingService.getMeetings().subscribe(
      (data) => {
        this.meetings = data;
        this.organizeData();
      },
      (error) => {
        console.error('Error fetching meetings:', error);
      }
    );
  }

  organizeData(): void {
    // Extract unique time slots
    const timeSet = new Set<string>();
    this.meetings.forEach(meeting => {
      const timeKey = `${this.formatTime(meeting.startTime)} - ${this.formatTime(meeting.finishTime)}`;
      timeSet.add(timeKey);
    });
    
    // Sort time slots chronologically
    this.timeSlots = Array.from(timeSet).sort((a, b) => {
      const timeA = this.meetings.find(m => 
        `${this.formatTime(m.startTime)} - ${this.formatTime(m.finishTime)}` === a)?.startTime || '';
      const timeB = this.meetings.find(m => 
        `${this.formatTime(m.startTime)} - ${this.formatTime(m.finishTime)}` === b)?.startTime || '';
      return new Date(timeA).getTime() - new Date(timeB).getTime();
    });

    // Extract unique table names
    const tableSet = new Set<string>();
    this.meetings.forEach(meeting => {
      tableSet.add(meeting.tableName);
    });
    
    // Sort tables by name
    this.tables = Array.from(tableSet).sort();

    // Organize meetings by table and time slot (for Time View)
    this.tables.forEach(table => {
      this.meetingsByTableAndTime[table] = {};
      
      this.timeSlots.forEach(timeSlot => {
        const meeting = this.meetings.find(m => 
          `${this.formatTime(m.startTime)} - ${this.formatTime(m.finishTime)}` === timeSlot && 
          m.tableName === table
        );
        
        this.meetingsByTableAndTime[table][timeSlot] = meeting as Meeting;
      });
    });
    
    // Organize meetings by time slot and table (for Table View)
    this.timeSlots.forEach(timeSlot => {
      this.meetingsByTimeAndTable[timeSlot] = {};
      
      this.tables.forEach(table => {
        const meeting = this.meetings.find(m => 
          `${this.formatTime(m.startTime)} - ${this.formatTime(m.finishTime)}` === timeSlot && 
          m.tableName === table
        );
        
        this.meetingsByTimeAndTable[timeSlot][table] = meeting as Meeting;
      });
    });
  }

  formatTime(dateTime: Date | string | null | undefined): string {
    if (!dateTime) {
      return '';
    }
    
    if (typeof dateTime === 'string') {
      return this.datePipe.transform(new Date(dateTime), 'shortTime') || '';
    }
    
    return this.datePipe.transform(dateTime, 'shortTime') || '';
  }

  confirmDelete(meeting: Meeting): void {
    this.selectedMeeting = meeting;
    this.showDeleteModal = true;
  }
  

  generateSchedule(eventId: number = 1): void {
    console.log('Generating schedule for event ID:', eventId);
    this.meetingService.generateSchedule(eventId).subscribe({
      next: (response) => {
        console.log('Schedule generated successfully:', response);
        // Reload meetings to show the new schedule
        this.loadMeetings();
      },
      error: (error) => {
        console.error('Error generating schedule:', error);
        // Show error to user
        alert('Failed to generate schedule: ' + (error.error?.message || error.message || 'Unknown error'));
      }
    });
  }
}
