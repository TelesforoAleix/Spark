import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { Login, LoginResponse } from '../model/login';
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  baseUrl: string = "http://localhost:5193/api";
  constructor(private http: HttpClient) {
  }
  authenticate(username: String, password: String): Observable<LoginResponse> {
    const credentials: Login = { username: username, password: password };

    return this.http.post<LoginResponse>(`${this.baseUrl}/Login`, credentials)
      .pipe(
        tap(response => {
          // Store auth data when login is successful
          localStorage.setItem('authToken', response.headerValue);
          localStorage.setItem('userId', response.userId.toString());
          localStorage.setItem('username', response.username);
        })
      );
  }

  isLoggedIn(): boolean {
    return localStorage.getItem('authToken') !== null;
  }

  logout(): void {
    localStorage.removeItem('authToken');
    localStorage.removeItem('userId');
    localStorage.removeItem('username');
  }
}