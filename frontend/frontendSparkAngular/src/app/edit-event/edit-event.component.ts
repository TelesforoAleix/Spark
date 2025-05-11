import { Component, OnInit } from '@angular/core';
import { EventService } from '../services/event.service';
import { Event } from '../model/event';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-edit-event',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './edit-event.component.html',
  styleUrl: './edit-event.component.css'
})
export class EditEventComponent implements OnInit {
  event!: Event;

  constructor(
    private eventService: EventService,
    private router: Router,
  ) {}

  ngOnInit() { 
    this.loadEvent();
  }

  loadEvent(): void {
    this.eventService.getEvent().subscribe(
      event => { 
        this.event = event;
        
        // Convert dates to the format required by datetime-local input
        if (this.event.startDate) {
          this.event.startDate = this.formatDateForInput(this.event.startDate);
        }
        if (this.event.finishDate) {
          this.event.finishDate = this.formatDateForInput(this.event.finishDate);
        }
        if (this.event.networkingStartDate) {
          this.event.networkingStartDate = this.formatDateForInput(this.event.networkingStartDate);
        }
        if (this.event.networkingFinishDate) {
          this.event.networkingFinishDate = this.formatDateForInput(this.event.networkingFinishDate);
        }
      },
      (error) => {
        console.error('Error fetching event:', error);
        alert('Error loading event information');
      }
    );
  }

  updateEvent(): void {
    // Validate required fields
    if (!this.event.name || !this.event.location || !this.event.startDate || !this.event.finishDate) {
      alert('Please fill in all required fields');
      return;
    }

   // Create a properly formatted event object for the backend
   const eventToUpdate = {
    eventId: Number(this.event.eventId),
    name: String(this.event.name).trim(),
    startDate: this.parseDateTime(this.event.startDate),
    finishDate: this.parseDateTime(this.event.finishDate),
    location: String(this.event.location).trim(),
    bio: String(this.event.bio || '').trim(),
    networkingStartDate: this.parseDateTime(this.event.networkingStartDate),
    networkingFinishDate: this.parseDateTime(this.event.networkingFinishDate),
    meetingDuration: parseInt(String(this.event.meetingDuration), 10) || 0,
    breakDuration: parseInt(String(this.event.breakDuration), 10) || 0,
    availableTables: parseInt(String(this.event.availableTables), 10) || 0
  };

  console.log('Sending event to update:', eventToUpdate); // Debug log

    this.eventService.updateEvent(eventToUpdate).subscribe(
      () => {
        console.log('Event updated successfully');
        this.router.navigate(['/event-info']);
      },
      (error) => {
        console.error('Error updating event:', error);
        alert('Error updating event information');
      }
    );
  }

  cancelEdit(): void {
    this.router.navigate(['/event-info']);
  }

  // Helper method to format dates for datetime-local input (frontend display)
  private formatDateForInput(date: any): string {
    if (!date) return '';
    
    try {
      let dateObj: Date;
      
      if (date instanceof Date) {
        dateObj = date;
      } else if (typeof date === 'string') {
        dateObj = new Date(date);
      } else {
        return '';
      }
      
      // Create local time string for datetime-local input
      const year = dateObj.getFullYear();
      const month = String(dateObj.getMonth() + 1).padStart(2, '0');
      const day = String(dateObj.getDate()).padStart(2, '0');
      const hours = String(dateObj.getHours()).padStart(2, '0');
      const minutes = String(dateObj.getMinutes()).padStart(2, '0');
      
      return `${year}-${month}-${day}T${hours}:${minutes}`;
    } catch (error) {
      console.error('Error formatting date for input:', error);
      return '';
    }
  }

  // Helper method to parse datetime-local input and ensure proper Date format
  private parseDateTime(dateStr: any): string {
    if (!dateStr) return new Date().toISOString(); // Default to current date if empty
    
    try {
      // If it's already a Date object, return ISO string
      if (dateStr instanceof Date) {
        return dateStr.toISOString();
      }
      
      // If it's a string, parse it and return ISO string
      const parsed = new Date(dateStr);
      if (isNaN(parsed.getTime())) {
        throw new Error('Invalid date');
      }
      
      return parsed.toISOString();
    } catch (error) {
      console.error('Error parsing date:', error, 'for value:', dateStr);
      return new Date().toISOString(); // Fallback to current date
    }
  }
}
