// create-workout-modal.component.ts
import { Component, Output, EventEmitter, ViewChild, ElementRef, OnInit } from '@angular/core';
import { Workout, WorkoutType } from '../../core/models/workout.models';
import { WorkoutService } from '../../core/services/workout.service';
import { AuthService } from '../../core/auth/auth.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  standalone: true,
  imports: [FormsModule, CommonModule],
  selector: 'app-create-workout-modal',
  templateUrl: './create-workout-modal.component.html'
})
export class CreateWorkoutModalComponent implements OnInit {
  @ViewChild('createWorkoutModal') modalRef!: ElementRef<HTMLDialogElement>;
  @Output() workoutCreated = new EventEmitter<void>();

  workoutTypes: { value: WorkoutType, label: string }[] = [
    { value: WorkoutType.Plank, label: 'Plank' },
    { value: WorkoutType.PushUp, label: 'Push up' },
    { value: WorkoutType.PullUp, label: 'Pull up' },
    { value: WorkoutType.SitUp, label: 'Sit up' },
    { value: WorkoutType.ChinUp, label: 'Chin up' },
  ];
  
  workout: Omit<Workout, 'id'> = {
    userId: '',
    tiredness: 1,
    difficulty: 1,
    caloriesSpent: 0,
    duration: 0,
    workoutDate: new Date().toISOString(),
    workoutType: WorkoutType.ChinUp,
    additionalNote: '',
  };

  constructor(
    private workoutService: WorkoutService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    const userId = this.authService.getUserId();
    if (userId) {
      this.workout.userId = userId;
    }
  }

  closeModal() {
    this.modalRef.nativeElement.close();
  }

  openModal() {
    this.modalRef.nativeElement.showModal();
  }

  createWorkout() {
    this.workoutService.createWorkout(this.workout).subscribe({
      next: () => {
        this.workoutCreated.emit();
        this.modalRef.nativeElement.close();
        this.resetWorkout();
      },
      error: err => {
        console.error('Error creating workout:', err);
      }
    });
  }

  private resetWorkout() {
    this.workout = {
      userId: this.authService.getUserId() ?? '',
      tiredness: 1,
      difficulty: 1,
      caloriesSpent: 0,
      duration: 0,
      workoutDate: new Date().toISOString(),
      workoutType: WorkoutType.ChinUp,
      additionalNote: ''
    };
  }  
}
