// workout-summary.service.ts

import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environments';
import { AuthService } from '../auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class WorkoutSummaryService {
  private apiUrl = environment.apiBaseUrl + '/Workout';

  constructor(private httpClient: HttpClient, private authService: AuthService) {}

  getWeeklySummary(userId: string, month: string, week: number): Observable<any> {
    const url = `${this.apiUrl}/summary/${userId}/${month}/${week}`;
    const token = this.authService.getAuthToken();
    const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<any[]>(url, {headers});
  }
}
