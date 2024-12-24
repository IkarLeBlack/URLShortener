import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { ShortUrlsTableComponent } from './pages/url-table/url-table.component';
import { ShortUrlInfoComponent } from './pages/url-info/url-info.component';
import { AboutComponent } from './pages/about/about.component';
import { AdminGuard } from './data/guards/admin.guard';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'urls', component: ShortUrlsTableComponent },
  { path: 'urls-info', component: ShortUrlInfoComponent },
  { path: 'about', component: AboutComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
