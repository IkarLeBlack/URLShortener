import { Component, OnInit, OnDestroy } from '@angular/core';
import { UrlService } from '../../data/services/url.service';
import { AuthService } from '../../data/services/auth.service';
import { UrlModel } from '../../data/interfaces/url.interface';
import { SignalrService } from '../../data/services/signalr.service';
import { Subscription } from 'rxjs';
import {NgForOf, NgIf} from '@angular/common';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-short-urls-table',
  templateUrl: './url-table.component.html',
  styleUrls: ['./url-table.component.scss'],
  imports: [
    NgIf,
    NgForOf
  ],
  standalone: true
})
export class ShortUrlsTableComponent implements OnInit, OnDestroy {
  urls: UrlModel[] = [];
  public isAuthorized: boolean = false;
  private signalRSubscription: Subscription | null = null;
  public currentUserId: number | null = null;
  public isAdmin: boolean = false;



  constructor(
    private urlService: UrlService,
    private authService: AuthService,
    private signalrService: SignalrService,
    private router: Router
  ) {}

  ngOnInit() {
    if (Notification.permission !== 'granted') {
      Notification.requestPermission();
    }
    this.isAuthorized = this.authService.isAuthenticated();
    this.loadUrls();
    this.currentUserId = this.authService.getCurrentUserId();
    this.isAdmin = this.authService.isAdmin();
    this.signalRSubscription = this.signalrService.updateUrls.subscribe(() => {
      this.loadUrls();
    });

    this.signalrService.startConnection();
  }

  ngOnDestroy() {
    if (this.signalRSubscription) {
      this.signalRSubscription.unsubscribe();
    }
  }

  loadUrls() {
    this.urlService.getUrls().subscribe({
      next: (data) => (this.urls = data),
      error: (err) => console.error('Failed to load URLs:', err),
    });
  }

  onAddUrl() {
    const newUrl = prompt('Enter the original URL:');
    if (newUrl) {
      this.urlService.addUrl({ originalUrl: newUrl }).subscribe({
        next: () => {
          this.loadUrls(); // Перезагружаем список URL
          if (Notification.permission === 'granted') {
            new Notification('Success', { body: 'Short URL created successfully!' });
          }
        },
        error: (err) => {
          if (err.error === 'This URL already exists.') {
            if (Notification.permission === 'granted') {
              new Notification('Error', { body: 'This URL already exists.' });
            }
          } else {
            if (Notification.permission === 'granted') {
              new Notification('Error', { body: 'Failed to add URL.' });
            }
          }
          console.error('Failed to add URL:', err);
        },
      });
    }
  }



onViewDetails(urlId: number) {
    this.router.navigate(['/urls-info', urlId]);
  }

  onDelete(urlId: number) {
    if (confirm('Are you sure you want to delete this URL?')) {
      this.urlService.deleteUrl(urlId).toPromise().then(() => {
        this.loadUrls();
      }).catch((err) => {
        console.error('Failed to delete URL:', err);
      });
    }
  }

  canDelete(url: UrlModel): boolean {
    return this.isAdmin || this.currentUserId === url.userId;
  }

  onGoToAlgorithmPage() {
    this.router.navigate(['/about']);
  }

  isAuth() {
    return this.isAuthorized
  }
}
