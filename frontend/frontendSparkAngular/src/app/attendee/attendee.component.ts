import { Component, Input } from '@angular/core';
import { Attendee } from '../model/attendee';

@Component({
  selector: 'app-attendee',
  standalone: true,
  imports: [],
  templateUrl: './attendee.component.html',
  styleUrl: './attendee.component.css'
})

export class AttendeeComponent {
  @Input() attendee?: Attendee;
}

