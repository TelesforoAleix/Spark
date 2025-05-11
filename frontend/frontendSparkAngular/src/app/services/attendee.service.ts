import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Attendee } from '../model/attendee';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class AttendeeService {
  baseUrl = 'http://localhost:5193/api';
  constructor(private http: HttpClient) {}

  get authHeader(): string { return localStorage["headerValue"]; }


    getAttendees(): Observable<Attendee[]> {
    console.log('AttendeeService getAttendees() called');
    return this.http.get<Attendee[]>(`${this.baseUrl}/Attendee`, {headers: {
      "Authorization": this.authHeader
    }
    });
    }
    getAttendee(id: string): Observable<Attendee> {
    console.log('AttendeeService getAttendee() called');
    return this.http.get<Attendee>(`${this.baseUrl}/Attendee/${id}`, {headers: {
      "Authorization": this.authHeader
    }
    });
    }

    updateAttendee(attendee: Attendee): Observable<any> {
    console.log('AttendeeService updateAttendee() called');
    return this.http.put(`${this.baseUrl}/Attendee/${attendee.attendeeId}`, attendee, {headers: {
      "Authorization": this.authHeader}
    });
    }

    createAttendee(attendee: Attendee): Observable<any> {
    console.log('AttendeeService createAttendee() called');
    return this.http.post(`${this.baseUrl}/Attendee`, attendee, {headers: {
      "Authorization": this.authHeader}
    });
    }

    deleteAttendee(id: string): Observable<any> {
    console.log('AttendeeService deleteAttendee() called');
    return this.http.delete(`${this.baseUrl}/Attendee/${id}`, {headers: {
      "Authorization": this.authHeader}
    });
    }
    
}