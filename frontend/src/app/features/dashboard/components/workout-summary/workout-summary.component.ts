import { Component, OnInit } from '@angular/core';
import { WorkoutSummaryService } from '../../../../core/services/workout-summary.service';
import { AuthService } from '../../../../core/auth/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  imports: [CommonModule, FormsModule],
  selector: 'app-workout-summary',
  templateUrl: './workout-summary.component.html',
})
export class WorkoutSummaryComponent implements OnInit {
  userId: string = '';
  month: string = '01';
  week: number = 1;
  weeklySummary: any = null; 
  loading: boolean = false;
  error: string = '';

  availableMonths: string[] = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

  constructor(
    private workoutSummaryService: WorkoutSummaryService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.userId = this.authService.getUserId();
    this.loadWeeklySummary();
  }

  loadWeeklySummary(): void {
    if (!this.userId) return;

    this.loading = true;
    const selectedMonth = this.month;
    const selectedWeek = this.week;

    this.workoutSummaryService.getWeeklySummary(this.userId, selectedMonth, selectedWeek).subscribe({
      next: (data) => {
        this.weeklySummary = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Failed to load weekly summary:', err);
        this.error = 'There was an issue loading the data.';
        this.loading = false;
      }
    });
  }

  onMonthChange(): void {
    this.weeklySummary = null;
    this.loadWeeklySummary();
  }

  onWeekChange(): void {
    this.weeklySummary = null;
    this.loadWeeklySummary(); 
  }
}
