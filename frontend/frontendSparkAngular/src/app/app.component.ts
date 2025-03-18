import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { AttendeeComponent } from './attendee/attendee.component';
import { AttendeeListComponent } from './attendee-list/attendee-list.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, AttendeeComponent, AttendeeListComponent, RouterLink],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'frontendSparkAngular';
}
