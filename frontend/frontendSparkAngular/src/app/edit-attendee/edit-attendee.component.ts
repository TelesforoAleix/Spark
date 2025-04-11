import { Component, Input, OnInit } from '@angular/core';
import { AttendeeService } from '../services/attendee.service';
import { Attendee } from '../model/attendee';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-edit-attendee',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './edit-attendee.component.html',
  styleUrl: './edit-attendee.component.css'
})
export class EditAttendeeComponent {
  @Input() attendeeId!: string;
  attendee!: Attendee;

  constructor(private attendeeService: AttendeeService) {}

  ngOnInit() { 
    this.attendeeService.getAttendee(this.attendeeId).subscribe(
      attendee => { 
        this.attendee = attendee;
      },
      (error) => {
        console.error('Error fetching attendee:', error);
      }
    );
  }

  deleteAttendee(): void {
    if (this.attendee?.attendeeId) {
      this.attendeeService.deleteAttendee(this.attendee.attendeeId).subscribe();
    } else {
      console.error('Attendee ID is undefined');
    }
    }

  updateAttendee() {
    this.attendeeService.updateAttendee(this.attendee!).subscribe(
      () => {
        console.log('Attendee updated successfully');
      },
      (error) => {
        console.error('Error updating attendee:', error);
        console.error(this.attendee);
      }
    );
  }
}
