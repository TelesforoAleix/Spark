import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router'; // ✅ import Router
import { LoginService } from '../services/login.service';
import { User } from '../model/user';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html'
})
export class LoginComponent {
  user: User = { username: '', password: '' };
  message: string = '';

  constructor(
    private loginService: LoginService,
    private router: Router // inject Router
  ) {}

  onSubmit(): void {
    this.loginService.login(this.user).subscribe({
      next: (res) => {
        this.message = res.message;
        this.router.navigate(['/event-info']); // navigate after login
      },
      error: (err) => this.message = err.error.message || 'Login failed'
    });
  }
}