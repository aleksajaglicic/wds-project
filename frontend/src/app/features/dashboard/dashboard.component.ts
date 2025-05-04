import { Component, OnInit } from '@angular/core';
import { WorkoutTableComponent } from './components/workout-table/workout-table.component';
import { WorkoutService } from '../../core/services/workout.service';
import { Workout } from '../../core/models/workout.models';
import { AuthService } from '../../core/auth/auth.service';
import { CommonModule } from '@angular/common';
import { WorkoutSummaryComponent } from './components/workout-summary/workout-summary.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, WorkoutTableComponent, WorkoutSummaryComponent],
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit {
  workouts: Workout[] = [];
  currentPage = 1;
  pageSize = 9;

  constructor(private workoutService: WorkoutService, private authService: AuthService) {}

  ngOnInit(): void {
    console.log("ng init")
    this.loadWorkouts();
  }

  loadWorkouts(): void {
    const userId = this.authService.getUserId();
    this.workoutService
      .getAllWorkouts(userId, this.currentPage, this.pageSize)
      .subscribe({
        next: (data: Workout[]) => {
          this.workouts = data;
        },
        error: (err: string) => {
          console.error('Failed to load workouts:', err);
        },
      });
  }

  onPageChanged(page: number): void {
    this.currentPage = page;
    this.loadWorkouts();
  }
}
