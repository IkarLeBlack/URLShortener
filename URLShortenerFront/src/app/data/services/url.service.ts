import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { UrlModel } from '../interfaces/url.interface';
import { AuthService } from './auth.service';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';
@Injectable({
  providedIn: 'root',
})
export class UrlService {
  private apiUrl = 'https://localhost:7150/api/urls';

  constructor(private https: HttpClient, private authService: AuthService) {}

  getUrls(): Observable<UrlModel[]> {
    return this.https.get<UrlModel[]>(this.apiUrl);
  }

  getUrlById(id: number): Observable<UrlModel> {
    return this.https.get<UrlModel>(`${this.apiUrl}/${id}`);
  }

  addUrl(url: Partial<UrlModel>): Observable<UrlModel | string> {
    const username = this.authService.getCurrentUser()?.username || '';
    const hashedUrl = this.hashUrl(url.originalUrl || '');
    const requestData = {
      username: username,
      hashedUrl: hashedUrl,
      originalUrl: url.originalUrl,
    };

    return this.https.post<UrlModel>(`${this.apiUrl}/create`, requestData).pipe(
      catchError((error) => {

        if (error.status === 400 && error.error === 'This URL already exists.') {
          return of('This URL already exists.');
        }

        return of('An unexpected error occurred.');
      })
    );
  }

  deleteUrl(id: number): Observable<void> {
    return this.https.delete<void>(`${this.apiUrl}/${id}`);
  }


  private hashUrl(url: string): string {
    return btoa(url);
  }
}
