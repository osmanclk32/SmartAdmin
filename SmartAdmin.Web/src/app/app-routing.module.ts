import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainLayoutComponent } from './shared/layout/app-layouts/main-layout/main-layout.component';
import { AuthLayoutComponent } from './shared/layout/app-layouts/main-layout/auth-layout.component';
import { AuthGuard } from './core/_helpers/auth.guard';

const routes: Routes = [
  {
    path: '', component: MainLayoutComponent, data: {pageTitle: 'Home'},canActivate: [AuthGuard]
  },
  {
    path: 'auth', component: AuthLayoutComponent,loadChildren: () => import('src/app/modules/auth/auth.module').then(m => m.AuthModule)
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
