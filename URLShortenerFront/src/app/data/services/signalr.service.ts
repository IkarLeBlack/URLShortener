import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { UrlService } from './url.service';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SignalrService {
  private hubConnection: signalR.HubConnection;
  public updateUrls: Subject<void> = new Subject();

  constructor(private urlService: UrlService) {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7150/urlUpdateHub', {
        withCredentials: true,
      })
      .build();
  }

  public startConnection(): void {
    this.hubConnection
      .start()
      .then(() => {
        console.log('SignalR connection established');
      })
      .catch((err) => console.error('SignalR connection error: ', err));

    this.hubConnection.on('UpdateUrls', () => {
      // При получении сообщения отправляем уведомление об обновлении
      this.updateUrls.next();
    });
  }
}
