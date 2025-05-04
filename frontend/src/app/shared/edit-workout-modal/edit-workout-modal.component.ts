import { CommonModule, DatePipe } from '@angular/common';
import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { WorkoutService } from '../../core/services/workout.service';
import { AuthService } from '../../core/auth/auth.service';
import { Workout, WorkoutType } from '../../core/models/workout.models';

@Component({
  standalone: true,
  selector: 'app-edit-workout-modal',
  imports: [FormsModule, CommonModule],
  providers: [DatePipe],
  templateUrl: './edit-workout-modal.component.html',
})
export class EditWorkoutModalComponent implements OnInit {
  @ViewChild('editWorkoutModal') modalRef!: ElementRef<HTMLDialogElement>;
  @Output() workoutUpdated = new EventEmitter<void>();
  @Input() workout!: Workout;

  constructor(private workoutService: WorkoutService, private authService: AuthService, private datePipe: DatePipe) { }

  workoutTypes: { value: WorkoutType, label: string }[] = [
      { value: WorkoutType.Plank, label: 'Plank' },
      { value: WorkoutType.PushUp, label: 'Push up' },
      { value: WorkoutType.PullUp, label: 'Pull up' },
      { value: WorkoutType.SitUp, label: 'Sit up' },
      { value: WorkoutType.ChinUp, label: 'Chin up' },
    ];

    openModal() {
      this.modalRef.nativeElement.showModal();
    }
  
    closeModal() {
      this.modalRef.nativeElement.close();
    }
  
    ngOnInit(): void {
      if (this.workout && this.workout.workoutDate) {
        this.workout.workoutDate = this.datePipe.transform(this.workout.workoutDate, 'M/d/yyyy HH:mm') || '';
      }
    }
    
    
    
  

    updateWorkout() {
      this.workoutService.editWorkout(this.workout).subscribe({
        next: () => {
          this.workoutUpdated.emit();
          this.closeModal();
        }      })
    }
}
