import { Injectable } from '@angular/core';
import { Workout, WorkoutType } from '../models/workout.models';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import { environment } from '../../../environments/environments';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class WorkoutService {
  private apiUrl = environment.apiBaseUrl + '/Workout';

  constructor(private httpClient: HttpClient, private authService: AuthService) { }

  getAllWorkouts(userId: string, pageNumber: number, numOfElements: number): Observable<Workout[]> {
    const url = `${this.apiUrl}/${userId}/${pageNumber}/${numOfElements}`;
    const token = this.authService.getAuthToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<Workout[]>(url, {headers});
  }

  deleteWorkout(id: string) {
    const url = `${this.apiUrl}/${id}`;
    const token = this.authService.getAuthToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.delete(url, {headers, responseType: 'text'});
  }

  createWorkout(workout: Omit<Workout, 'id'>): Observable<any> {
    const token = this.authService.getAuthToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post(this.apiUrl, workout, { headers });
  }

  editWorkout(workout: Workout): Observable<any> {
    const token = this.authService.getAuthToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.put(this.apiUrl, workout, { headers });
  }
}
