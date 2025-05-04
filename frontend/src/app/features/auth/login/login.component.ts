import { Component, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { JwtPayload, LoginRequest } from '../../../core/auth/auth.model';
import { AuthService } from '../../../core/auth/auth.service';
import { jwtDecode } from 'jwt-decode';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
})
export class LoginComponent {
  loginForm: FormGroup;
  formSubmitted = signal(false);

  constructor(private fb: FormBuilder, private authService: AuthService, private _router: Router) {
    this.loginForm = this.fb.group({
      'email': [''],
      'password': ['']
    })}

  ngOnInit() {
    if(this.authService.isLoggedIn()) {
      this._router.navigate(['dashboard']);
    }
  }
  
  login(event: Event) {
    event.preventDefault();

    if(this.loginForm.valid) {
      const loginData: LoginRequest = this.loginForm.value;

      console.log('Form submitted:', this.loginForm.value);
      this.formSubmitted.set(true);

      this.authService.login(loginData).subscribe(
        (response) => {
          console.log('Login successful: ', response);

          const token = response.token;
          const decoded = jwtDecode<JwtPayload>(token);
          const currentTime = Date.now() / 1000;
          
          localStorage.setItem('authToken', token);
          if(decoded.exp > currentTime) {
            this._router.navigate(["dashboard"]);
          }
        }
      )
    } else {
      console.log('Form is invalid');
    }
  }
}
