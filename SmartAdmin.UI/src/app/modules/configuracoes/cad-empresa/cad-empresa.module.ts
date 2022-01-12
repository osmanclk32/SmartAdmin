import { NgModule,ViewContainerRef,ComponentFactoryResolver,OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CadEmpresaComponent } from './cad-empresa.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [{
    path: '',
    component: CadEmpresaComponent
}, ]

@NgModule(
{
  declarations: [CadEmpresaComponent],
  imports: [CommonModule,
            RouterModule.forChild(routes) ],

})

export class CadEmpresaModule {}

