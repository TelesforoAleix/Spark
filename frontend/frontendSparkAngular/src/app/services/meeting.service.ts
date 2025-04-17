// Service: Handles all HTTP requests related to Meeting data
// Talks to the backend API at /api/meeting
// Makes it easy for components to load and create meetings without repeating logic

import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Meeting } from '../model/meeting.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root' // Makes this service globally available without needing to import it in providers
})
export class MeetingService {
  private baseUrl = 'http://localhost:5193/api/meeting'; // Backend endpoint for MeetingController

  constructor(private http: HttpClient) {}

  // Fetches all meetings from the backend
  getAllMeetings(): Observable<Meeting[]> {
    return this.http.get<Meeting[]>(`${this.baseUrl}`);
  }

  // Fetches meetings for a specific attendee
  getMeetingsByAttendee(attendeeId: number): Observable<Meeting[]> {
    return this.http.get<Meeting[]>(`${this.baseUrl}/attendee/${attendeeId}`);
  }

  // Fetches meetings assigned to a specific table
  getMeetingsByTable(table: string): Observable<Meeting[]> {
    return this.http.get<Meeting[]>(`${this.baseUrl}/table/${table}`);
  }

  // Creates a new meeting in the backend
  createMeeting(meeting: Meeting): Observable<any> {
    return this.http.post(`${this.baseUrl}`, meeting);
  }
}
