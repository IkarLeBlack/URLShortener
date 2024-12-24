import { Component } from '@angular/core';
import { AuthService } from '../../data/services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import {CommonModule, NgIf} from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  imports: [
    FormsModule,
    NgIf
  ],
  standalone: true
})
export class LoginComponent {
  loginData = { username: '', password: '' };
  errorMessage: string | null = null;

  constructor(private authService: AuthService, private router: Router) {}

  onLogin() {
    this.authService.login(this.loginData).subscribe({
      next: (response) => {
        if (response) {
          console.log('Login successful:', response);
          this.errorMessage = null;
          this.router.navigate(['/urls']);
        } else {
          console.error('Login failed: Invalid credentials');
          this.errorMessage = 'Incorrect Password.';
        }
      },
      error: (err) => {
        console.error('Login failed:', err);
        this.errorMessage = 'An error occurred. Please try again.';
      }
    });
  }

}
