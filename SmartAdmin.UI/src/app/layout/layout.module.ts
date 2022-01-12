import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LayoutRoutingModule } from './layout-routing.module';
import { MainLayoutComponent } from './main-layout/main-layout.component';
import { ContentComponent } from './main-layout/content/content.component';
import { ControlSideBarComponent } from './main-layout/control-side-bar/control-side-bar.component';
import { MainSideBarComponent } from './main-layout/main-side-bar/main-side-bar.component';
import { NavBarComponent } from './main-layout/nav-bar/nav-bar.component';
import { FooterComponent } from './main-layout/footer/footer.component';


@NgModule({
  declarations: [MainLayoutComponent, ContentComponent, ControlSideBarComponent, MainSideBarComponent, NavBarComponent, FooterComponent],
  imports: [
    CommonModule,
    LayoutRoutingModule
  ]
})
export class LayoutModule { }
