import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Meeting } from '../model/meeting';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class MeetingService {
  baseUrl = 'http://localhost:5193/api';
  constructor(private http: HttpClient) { }

  get authHeader(): string { return localStorage["headerValue"]; }

  // Get all meetings
  getMeetings(): Observable<Meeting[]> {
    console.log('MeetingService getMeetings() called');
    return this.http.get<Meeting[]>(`${this.baseUrl}/Meeting`, {
      headers: {
        "Authorization": this.authHeader
      }
    });
  }

  // Generate new schedule
  generateSchedule(eventId: number = 1): Observable<any> {
    console.log('MeetingService generateSchedule() called');
    return this.http.post(`${this.baseUrl}/Meeting/GenerateSchedule`, {eventId}, { 
  headers: {
    "Authorization": this.authHeader
  }
  });
}
  




}