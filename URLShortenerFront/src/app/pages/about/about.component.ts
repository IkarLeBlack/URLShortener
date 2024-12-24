import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../data/services/auth.service';
import { AboutService } from '../../data/services/about.service';
import {FormsModule} from '@angular/forms';
import {NgIf} from '@angular/common';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css'],
  standalone: true,
  imports: [
    FormsModule,
    NgIf
  ]
})
export class AboutComponent implements OnInit {
  aboutText: string = 'Loading...';
  isAdmin: boolean = false;

  constructor(
    private authService: AuthService,
    private aboutService: AboutService
  ) {}

  ngOnInit() {
    this.isAdmin = this.authService.isAdmin();
    this.loadAboutText();
  }

  loadAboutText() {
    this.aboutService.getDescription().subscribe({
      next: (description) => {
        this.aboutText = description;
        console.log('Loaded about text:', description);
      },
      error: (err) => {
        console.error('Failed to load about text:', err);
        if (err.status === 200) {
          console.log('Response body:', err.error);
        }
        this.aboutText = 'Failed to load description';
      }
    });
  }



  onSave() {
    if (this.isAdmin) {
      this.aboutService.updateDescription(this.aboutText).subscribe({
        next: () => {
          console.log('Description saved successfully');
        },
        error: (err) => {
          console.error('Error saving description', err);
        }
      });
    }
  }
}
