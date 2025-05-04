import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environments';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';
import { JwtPayload, LoginRequest, RegisterRequest } from './auth.model';
import { jwtDecode }from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiBaseUrl + '/Auth';
  constructor(private http: HttpClient) { }

  login(data: LoginRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, data);
  }

  isLoggedIn(): boolean {
    const token = localStorage.getItem('authToken');

    if(!token) {
      return false;
    }

    try {
      const decoded = jwtDecode<JwtPayload>(token);
      const currentTime = Date.now() / 1000;


      return decoded.exp > currentTime;
    } catch(e) {
      return false;
    }
  }

  getAuthToken(): string | null {
    return localStorage.getItem('authToken');
  }
  
  getUserId(): string {
    const token = localStorage.getItem('authToken');
    if (!token) {
      return '';
    }
  
    try {
      const decoded = jwtDecode<JwtPayload>(token);
      console.log('Decoded token:', decoded);
      const userId = decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
      console.log('User ID:', userId);
      return userId || '';
    } catch (e) {
      console.error('Error decoding token:', e);
      return '';
    }
  }

  logout() {
    localStorage.removeItem('authToken');
  }

  register(data: RegisterRequest): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, data, {responseType: 'text' as 'json'});
  }
}
