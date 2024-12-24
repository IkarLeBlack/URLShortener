import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UrlService } from '../../data/services/url.service';
import { UrlModel } from '../../data/interfaces/url.interface';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-short-url-info',
  templateUrl: './url-info.component.html',
  styleUrls: ['./url-info.component.scss'],
  imports: [DatePipe],
  standalone: true,
})
export class ShortUrlInfoComponent implements OnInit {
  urlInfo!: UrlModel;

  constructor(
    private route: ActivatedRoute,
    private urlService: UrlService,
    private router: Router
  ) {}

  ngOnInit(): void {
    const urlId = Number(this.route.snapshot.paramMap.get('id'));
    if (urlId) {
      this.urlService.getUrlById(urlId).subscribe({
        next: (data) => {
          this.urlInfo = data;
        },
        error: (err) => {
          console.error('Failed to load URL info:', err);
        },
      });
    } else {
      console.error('Invalid URL ID');
    }
  }

  onBack(): void {
    this.router.navigate(['/urls']);
  }
}
