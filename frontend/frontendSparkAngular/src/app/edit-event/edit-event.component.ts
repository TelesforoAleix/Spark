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
  networkingDate: string = '';
  networkingStartTime: string = '';
  networkingEndTime: string = '';

  constructor(
    private eventService: EventService,
    private router: Router,
  ) {}

  ngOnInit() { 
    this.loadEvent();

    this.eventService.getEvent().subscribe(
      (data) => {
        this.event = data;
        
        // Initialize networking date and times from existing data
        if (this.event.networkingStartDate) {
          const startDate = new Date(this.event.networkingStartDate);
          
          // Format date as YYYY-MM-DD for date input
          this.networkingDate = startDate.toISOString().split('T')[0];
          
          // Format time as HH:MM for time input
          this.networkingStartTime = 
            `${String(startDate.getHours()).padStart(2, '0')}:${String(startDate.getMinutes()).padStart(2, '0')}`;
        }
        
        if (this.event.networkingFinishDate) {
          const endDate = new Date(this.event.networkingFinishDate);
          
          // Format time as HH:MM for time input
          this.networkingEndTime = 
            `${String(endDate.getHours()).padStart(2, '0')}:${String(endDate.getMinutes()).padStart(2, '0')}`;
        }
      },
      (error) => {
        console.error('Error fetching event:', error);
      }
    );
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

    if (this.networkingDate) {
      // For networkingStartDate - combine date with start time
      if (this.networkingStartTime) {
        const [startHours, startMinutes] = this.networkingStartTime.split(':').map(Number);
        const startDateTime = new Date(this.networkingDate);
        startDateTime.setHours(startHours, startMinutes, 0);
        this.event.networkingStartDate = startDateTime;
      }
      
      // For networkingFinishDate - combine date with end time
      if (this.networkingEndTime) {
        const [endHours, endMinutes] = this.networkingEndTime.split(':').map(Number);
        const endDateTime = new Date(this.networkingDate);
        endDateTime.setHours(endHours, endMinutes, 0);
        this.event.networkingFinishDate = endDateTime;
      }
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
    if (!dateStr) return new Date().toISOString();
    
    try {
      let dateObj: Date;
      
      // Handle different input types
      if (dateStr instanceof Date) {
        dateObj = dateStr;
      } else if (typeof dateStr === 'string') {
        // For datetime-local input format (YYYY-MM-DDTHH:MM)
        if (dateStr.includes('T')) {
          // Ensure we have seconds
          if (dateStr.split(':').length === 2) {
            dateStr = `${dateStr}:00`;
          }
        }
        dateObj = new Date(dateStr);
      } else {
        console.warn('Unknown date format:', dateStr);
        return new Date().toISOString();
      }
      
      if (isNaN(dateObj.getTime())) {
        console.warn('Invalid date:', dateStr);
        return new Date().toISOString();
      }
      
      // Create a string that preserves the exact time without timezone conversion
      const year = dateObj.getFullYear();
      const month = String(dateObj.getMonth() + 1).padStart(2, '0');
      const day = String(dateObj.getDate()).padStart(2, '0');
      const hours = String(dateObj.getHours()).padStart(2, '0');
      const minutes = String(dateObj.getMinutes()).padStart(2, '0');
      const seconds = String(dateObj.getSeconds()).padStart(2, '0');
      
      // ISO format without timezone specifier
      const isoDate = `${year}-${month}-${day}T${hours}:${minutes}:${seconds}`;
      console.log(`Parsed date: ${dateStr} -> ${isoDate} (local time preserved)`);
      return isoDate;
    } catch (error) {
      console.error('Error parsing date:', error);
      return new Date().toISOString();
    }
  }
}
