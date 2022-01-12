import { ContentComponent } from './main-layout/content/content.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { MainLayoutComponent } from './main-layout/main-layout.component';
import { AuthGuard } from '@app/core/_helpers/auth.guard';

const routes: Routes = [
  {
    path: '',
    component: MainLayoutComponent,
    children: [
      {  path: '',  component: ContentComponent, },
      {  path: 'configuracoes/empresa', loadChildren: () => import('src/app/modules/configuracoes/cad-empresa/cad-empresa.module').then(m => m.CadEmpresaModule),canActivate: [AuthGuard]}
    ],

  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})

export class LayoutRoutingModule { }
