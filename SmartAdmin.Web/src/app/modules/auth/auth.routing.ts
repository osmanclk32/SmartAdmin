import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  {
    path: 'login',
    loadChildren: () => import('src/app/modules/auth/login/login.module').then(m => m.LoginModule)
  }
];

export const AuthRoutes = RouterModule.forChild(routes);
