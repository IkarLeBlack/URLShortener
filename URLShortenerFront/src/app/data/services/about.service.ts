import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AboutService {
  private apiUrl = 'https://localhost:7150/api/about';

  constructor(private http: HttpClient) {}

  getDescription(): Observable<string> {
    return this.http.get<string>(`${this.apiUrl}/description`, { responseType: 'text' as 'json' });
  }

  updateDescription(description: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/description`, { description });
  }
}
