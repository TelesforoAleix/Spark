import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../model/user';

@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private apiUrl = 'http://localhost:5193/api/Login';

  constructor(private http: HttpClient) {}

  login(user: User): Observable<any> {
    return this.http.post(this.apiUrl, user);
  }
}