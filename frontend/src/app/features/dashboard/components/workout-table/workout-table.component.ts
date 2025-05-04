import { Component, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { Workout, WorkoutType } from '../../../../core/models/workout.models';
import { WorkoutService } from '../../../../core/services/workout.service';
import { AuthService } from '../../../../core/auth/auth.service';
import { CommonModule, DatePipe } from '@angular/common';
import { Router } from '@angular/router';
import { CreateWorkoutModalComponent } from "../../../../shared/create-workout-modal/create-workout-modal.component";
import { EditWorkoutModalComponent } from '../../../../shared/edit-workout-modal/edit-workout-modal.component';
import { UserService } from '../../../../core/services/user.service';

@Component({
  selector: 'app-workout-table',
  standalone: true,
  imports: [CommonModule, CreateWorkoutModalComponent, EditWorkoutModalComponent],
  providers: [DatePipe],
  templateUrl: './workout-table.component.html',
})
export class WorkoutTableComponent {
  @Input() workouts: Workout[] = [];
  @Input() currentPage: number = 1;
  @Input() pageSize: number = 9;
  @Output() pageChanged: EventEmitter<number> = new EventEmitter<number>();
  @ViewChild('createWorkoutModal') createWorkoutModalRef!: CreateWorkoutModalComponent;
  @ViewChild('editWorkoutModal') editWorkoutModalRef!: EditWorkoutModalComponent

  selectedWorkout: Workout | null = null;

  constructor(private workoutService: WorkoutService, private authService: AuthService, private userService: UserService, private datePipe: DatePipe
  ) {}

  formatDate(date: string): string {
    return this.datePipe.transform(date, 'M/d/yyyy HH:mm') || '';
  }

  loadWorkouts(): void {
    const userId = this.authService.getUserId();
    if(!userId) return;

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
      console.log(this.workouts);
    }

  deleteWorkout(id: string) {
    const userId = this.authService.getUserId();
    if(!userId) return;

    console.log("trying to delete");
    this.workoutService.deleteWorkout(id).subscribe({
      next: () => {
        console.log("Workout deleted successfully");

        this.userService.deleteUserWorkout(userId, id).subscribe({
          next: () => {
            console.log("Workout Id removed from user's workout list.")
            this.loadWorkouts();
          }
        })
      },
      error: (err) => {
        console.error("Failed to delete workout:", err);
      }
    });
  }
  
  openCreateWorkoutModal(): void {
    this.createWorkoutModalRef.openModal();
  }

  openEditWorkoutModal(workout: Workout): void {
    this.selectedWorkout = workout;
    this.editWorkoutModalRef.openModal();
    console.log(this.workouts);
  }


  nextPage(): void {
    if (this.workouts.length === this.pageSize) {
      this.currentPage++;
      this.pageChanged.emit(this.currentPage);
      this.loadWorkouts();
    }
  }
  
  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.pageChanged.emit(this.currentPage);
      this.loadWorkouts();
    }
  }
}
