import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header.component';
import { LogoModule } from '../logo/logo.module';
import { DropdownMenuUserComponent } from '../header/dropdown-menu-user/dropdown-menu-user.component'

@NgModule({
  imports: [
    CommonModule,
    LogoModule
  ],
  declarations:
  [
    HeaderComponent,
    DropdownMenuUserComponent,
  ],
  exports:
  [HeaderComponent,
   DropdownMenuUserComponent
  ]
})
export class HeaderModule { }
