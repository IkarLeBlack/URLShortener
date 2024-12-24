import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { tap, catchError } from 'rxjs/operators';
import { UserModel } from '../interfaces/user.interface';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = 'https://localhost:7150/api/auth';
  private currentUser: UserModel | null = null;

  constructor(private https: HttpClient) {}

  login(credentials: { username: string; password: string }): Observable<UserModel | null> {
    const loginData = {
      Username: credentials.username,
      PasswordHash: this.hashPassword(credentials.password),
    };

    return this.https.post<UserModel>(`${this.apiUrl}/authenticate`, loginData).pipe(
      tap((user) => {
        this.currentUser = user;
        localStorage.setItem('user', JSON.stringify(user));
      }),
      catchError((error) => {
        console.error('Ошибка входа:', error);
        return of(null);
      })
    );
  }


  private hashPassword(password: string): string {
    return btoa(password);
  }

  getCurrentUser(): UserModel | null {
    if (!this.currentUser) {
      const userData = localStorage.getItem('user');
      if (userData) {
        this.currentUser = JSON.parse(userData);
      }
    }
    return this.currentUser;
  }

  isAdmin(): boolean {
    const user = this.getCurrentUser();
    return user?.role?.name === 'Admin';
  }

  isAuthenticated(): boolean {
    return this.getCurrentUser() !== null;
  }
  getCurrentUserId(): number | null {
    const user = this.getCurrentUser();
    return user ? user.id : null;
  }
}
