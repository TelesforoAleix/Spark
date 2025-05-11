import { Component, Input } from '@angular/core';
import { EventService } from '../services/event.service';
import { Event } from '../model/event';
import { Router } from '@angular/router';
import { CommonModule, DatePipe } from '@angular/common';

@Component({
  selector: 'app-event-info',
  standalone: true,
  imports: [CommonModule, DatePipe],
  templateUrl: './event-info.component.html',
  styleUrl: './event-info.component.css'
})
export class EventInfoComponent {
  @Input() 
  event!: Event;

  constructor(
    private eventService: EventService,
    private router : Router,
  ) {}

  ngOnInit() { 

    if (this.eventService.authHeader == null) { 
      this.router.navigate(["login"]); 
      return; 
    }
    this.eventService.getEvent().subscribe(
      event => { 
        this.event = event;
      },
      (error) => {
        console.error('Error fetching event:', error);
      }
    );
  }

  updateEvent() {
    this.eventService.updateEvent(this.event!).subscribe(
      () => {
        console.log('Event updated successfully');
        this.router.navigate(['/event']);
      },
      (error) => {
        console.error('Error updating event:', error);
        console.error(this.event);
      }
    );
  }

  editEvent(): void {
    console.log('Edit event clicked');
    this.router.navigate(['/edit-event']);
  }


}

