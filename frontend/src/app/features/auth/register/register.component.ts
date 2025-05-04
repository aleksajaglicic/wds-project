import { Component, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from '../../../core/auth/auth.service';
import { RegisterRequest } from '../../../core/auth/auth.model';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './register.component.html',
})
export class RegisterComponent {
  registerForm: FormGroup;
  formSubmitted = signal(false);
  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.registerForm = this.fb.group({
      'name': [''],
      'lastName': [''],
      'email': [''],
      'password': ['']
    })}

  ngOnInit() {

  }

  register(event: Event) {
    event.preventDefault();

    if(this.registerForm.valid) {
      const registerData: RegisterRequest = this.registerForm.value;

      this.authService.register(registerData).subscribe(
        (response) => {
          console.log('Registration successful: ', response);
          
          this.router.navigate(['/login']);
        }
      )

      console.log('Form submitted:', this.registerForm.value);
      this.formSubmitted.set(true);
    } else {
      console.log('Form is invalid');
    }
  }
}
