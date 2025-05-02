import { Component, Input, OnInit } from '@angular/core';
import { AttendeeService } from '../services/attendee.service';
import { Attendee } from '../model/attendee';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

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

  constructor(
    private attendeeService: AttendeeService,
    private router : Router,
  ) {}

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
      console.log('Attendee deleted successfully');
      this.router.navigate(['/attendee']);
    }else {
      console.error('Attendee ID is undefined');
    }
    }

  updateAttendee() {
    this.attendeeService.updateAttendee(this.attendee!).subscribe(
      () => {
        console.log('Attendee updated successfully');
        this.router.navigate(['/attendee']);
      },
      (error) => {
        console.error('Error updating attendee:', error);
        console.error(this.attendee);
      }
    );
  }

  cancelEdit(): void {
    this.router.navigate(['/attendee']);
  }
  
}
