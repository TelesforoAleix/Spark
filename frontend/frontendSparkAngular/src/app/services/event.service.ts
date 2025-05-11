import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Event } from '../model/event';


@Injectable({
  providedIn: 'root'
})
export class EventService {

  baseUrl = 'http://localhost:5193/api';
  constructor(private http: HttpClient) {}

  get authHeader(): string { return localStorage["headerValue"]; }

  getEvent(): Observable<Event> {
    console.log('EventService getEvent() called');
    return this.http.get<Event>(`${this.baseUrl}/Event`, {headers: {
      "Authorization": this.authHeader  }
    });
  }

  updateEvent(event: Event): Observable<any> {
    console.log('EventService updateEvent() called');
    return this.http.put(`${this.baseUrl}/Event`, event, 
      {
        headers: {
          "Authorization": this.authHeader
        }
      }
    );
  }

}

/*

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

    getAttendees(): Observable<Attendee[]> {
    console.log('AttendeeService getAttendees() called');
    return this.http.get<Attendee[]>(`${this.baseUrl}/Attendee`);
    }
    getAttendee(id: string): Observable<Attendee> {
    console.log('AttendeeService getAttendee() called');
    return this.http.get<Attendee>(`${this.baseUrl}/Attendee/${id}`);
    }
    updateAttendee(attendee: Attendee): Observable<any> {
    console.log('AttendeeService updateAttendee() called');
    return this.http.put(`${this.baseUrl}/Attendee/${attendee.attendeeId}`, attendee);
    }
    createAttendee(attendee: Attendee): Observable<any> {
    console.log('AttendeeService createAttendee() called');
    return this.http.post(`${this.baseUrl}/Attendee`, attendee);
    }
    deleteAttendee(id: string): Observable<any> {
    console.log('AttendeeService deleteAttendee() called');
    return this.http.delete(`${this.baseUrl}/Attendee/${id}`);
    }
}

*/ 