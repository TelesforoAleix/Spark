import { Routes } from '@angular/router';
import { AttendeeListComponent } from './attendee-list/attendee-list.component';
import { EditAttendeeComponent } from './edit-attendee/edit-attendee.component';

export const routes: Routes = [
    {path: "attendee", component: AttendeeListComponent},
    {path: "edit-attendee/:attendeeId", component: EditAttendeeComponent}, 

];
