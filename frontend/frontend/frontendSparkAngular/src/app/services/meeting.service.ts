import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Meeting } from '../model/meeting';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class MeetingService {
  baseUrl = 'http://localhost:5193/api';
  constructor(private http: HttpClient) {}

    getMeetings(): Observable<Meeting[]> {
    console.log('MeetingService getMeetings() called');
    return this.http.get<Meeting[]>(`${this.baseUrl}/Meeting`);
    }
    getMeeting(id: string): Observable<Meeting> {
    console.log('MeetingService getMeeting() called');
    console.log(id);
    return this.http.get<Meeting>(`${this.baseUrl}/Meeting/${id}`);
    }
    updateMeeting(meeting: Meeting): Observable<any> {
    console.log('MeetingService updateMeeting() called');
    console.log(meeting);
    return this.http.put(`${this.baseUrl}/Meeting/${meeting.meetingId}`, meeting);
    }
    createMeetings(meeting: Meeting): Observable<any> {
    console.log('MeetingService createMeeting() called');
    console.log(meeting);
    return this.http.post('${this.baseUrl}/Meeting', meeting);
    }
    deleteMeeting(id: string): Observable<any> {
    console.log('MeetingService deleteMeeting() called');
    console.log(id);
    return this.http.delete(`${this.baseUrl}/Meeting/${id}`);
    }
}