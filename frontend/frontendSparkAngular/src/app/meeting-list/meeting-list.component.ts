import { Component, OnInit } from '@angular/core';
import { Meeting } from '../model/meeting';
import { MeetingService } from '../services/meeting.service';
import { Router } from '@angular/router';
import { DatePipe, NgForOf, NgIf, NgClass, CommonModule } from '@angular/common';

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
  
  deleteMeeting(): void {
    if (this.selectedMeeting && this.selectedMeeting.meetingId) {
      this.meetingService.deleteMeeting(this.selectedMeeting.meetingId.toString()).subscribe(
        () => {
          this.meetings = this.meetings.filter(m => m.meetingId !== this.selectedMeeting?.meetingId);
          this.organizeData();
          this.showDeleteModal = false;
        },
        (error) => {
          console.error('Error deleting meeting:', error);
        }
      );
    }
  }

  editMeeting(meetingId: string | undefined): void {
    if (meetingId) {
      this.router.navigate(['/edit-meeting', meetingId]);
    }
  }
  
  addNewMeeting(): void {
    this.router.navigate(['/add-meeting']);
  }
  
  generateSchedule(): void {
    // Implement your schedule generation logic
    console.log('Generate schedule clicked');
  }
}
