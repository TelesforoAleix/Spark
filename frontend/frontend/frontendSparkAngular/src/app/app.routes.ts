import { Routes } from '@angular/router';
import { AttendeeListComponent } from './attendee-list/attendee-list.component';
import { EditAttendeeComponent } from './edit-attendee/edit-attendee.component';
import { EventInfoComponent } from './event-info/event-info.component';
import { MeetingListComponent } from './meeting-list/meeting-list.component';
import { LoginComponent } from './login/login.component'; // import LoginComponent

export const routes: Routes = [
  { path: '', component: LoginComponent }, // default route shows login
  { path: 'attendee', component: AttendeeListComponent }, // shows attendee list
  { path: 'edit-attendee/:attendeeId', component: EditAttendeeComponent }, // for editing an attendee
  { path: 'event-info', component: EventInfoComponent }, // event details page
  { path: 'meetings', component: MeetingListComponent }, // list of meetings
];