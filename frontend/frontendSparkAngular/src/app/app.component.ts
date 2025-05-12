import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AttendeeListComponent } from './attendee-list/attendee-list.component';
import { EventInfoComponent } from './event-info/event-info.component';
import { MeetingListComponent } from './meeting-list/meeting-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AttendeeListComponent, EventInfoComponent, RouterLink, MeetingListComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'frontendSparkAngular';
  
  logout() {
    localStorage.removeItem('headerValue');
    localStorage.removeItem('authToken');
  }
}

