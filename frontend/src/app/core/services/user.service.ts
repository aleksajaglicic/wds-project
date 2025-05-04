import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environments';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { User } from '../models/user.models';
import { Observable } from 'rxjs';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = environment.apiBaseUrl + '/User';

  constructor(private httpClient: HttpClient, private authService: AuthService) { }

  updateUser(data: User) : Observable<any> {
    return this.httpClient.put(`${this.apiUrl}`, data);
  }

  getUsers() : Observable<any> {
    return this.httpClient.get(`${this.apiUrl}`);
  }

  getUserById() : Observable<any> {
    return this.httpClient.get(`${this.apiUrl}`);
  }

  deleteUserWorkout(data: string, dataWorkout: string) : Observable<any> {
    const token = this.authService.getAuthToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.put(`${this.apiUrl}/${data}/${dataWorkout}`, data, {headers});
  }
}
