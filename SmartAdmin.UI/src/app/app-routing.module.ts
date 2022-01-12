import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/_helpers/auth.guard';
//import { AuthLayoutComponent } from './layout/main-layout/auth-layout.component';
//import { CadEmpresaComponent } from './modules/configuracoes/cad-empresa/cad-empresa.component';

const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./layout/layout.module').then(m => m.LayoutModule),canActivate: [AuthGuard],
  },
  { path: 'auth', loadChildren: () => import('src/app/modules/auth/auth.module').then(m => m.AuthModule)},


];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})

export class AppRoutingModule { }
