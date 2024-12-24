import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './pages/login/login.component';
import { ShortUrlsTableComponent } from './pages/url-table/url-table.component';
import { ShortUrlInfoComponent } from './pages/url-info/url-info.component';
import { AboutComponent } from './pages/about/about.component';;
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';


@NgModule({

  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    AppComponent,
    LoginComponent,
    ShortUrlsTableComponent,
    ShortUrlInfoComponent,
    AboutComponent,
    RouterModule.forRoot([])
  ],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}
