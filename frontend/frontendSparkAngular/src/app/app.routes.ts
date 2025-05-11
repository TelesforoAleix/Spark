import { Routes } from '@angular/router';
import { AttendeeListComponent } from './attendee-list/attendee-list.component';
import { EditAttendeeComponent } from './edit-attendee/edit-attendee.component';
import { EventInfoComponent } from './event-info/event-info.component';
import { MeetingListComponent } from './meeting-list/meeting-list.component';
import { AddAttendeeComponent } from './add-attendee/add-attendee.component';
import { LoginComponent } from './login/login.component';
import { EditEventComponent } from './edit-event/edit-event.component';


export const routes: Routes = [
    {path: "", redirectTo: "event-info", pathMatch: "full"},
    {path: "attendee", component: AttendeeListComponent},
    {path: "edit-attendee/:attendeeId", component: EditAttendeeComponent}, 
    {path: "event-info", component: EventInfoComponent},
    {path: "meetings", component: MeetingListComponent},
    {path: "add-attendee", component: AddAttendeeComponent},
    {path: "login", component: LoginComponent},
    {path: "edit-event", component: EditEventComponent} 
];
