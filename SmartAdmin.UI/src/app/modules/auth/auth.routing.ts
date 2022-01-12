import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from '@app/core/_helpers/auth.guard';

const routes: Routes = [
  {
    path: 'login',
    loadChildren: () => import('src/app/modules/auth/login/login.module').then(m => m.LoginModule),
  },

];

export const AuthRoutes = RouterModule.forChild(routes);
