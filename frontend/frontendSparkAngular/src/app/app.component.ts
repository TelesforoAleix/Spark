import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AttendeeComponent } from './attendee/attendee.component';
import { AttendeeListComponent } from './attendee-list/attendee-list.component';
import { EventInfoComponent } from './event-info/event-info.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AttendeeComponent, AttendeeListComponent, EventInfoComponent, RouterLink],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'frontendSparkAngular';
}
