import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterModule} from "@angular/router";
import { LeftPanelComponent } from './left-panel.component';
import { NavFilterComponent } from './nav-filter/nav-filter.component';
import { NavInfoCardComponent } from './nav-info-card/nav-info-card.component';
import { NavFooterComponent } from './nav-footer/nav-footer.component';
import { LogoModule } from '../../logo/logo.module';


@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    LogoModule
  ],
  declarations:
  [ LeftPanelComponent,
    NavFilterComponent,
    NavInfoCardComponent,
    NavFooterComponent,
  ],
  exports:
  [ LeftPanelComponent,
     NavFilterComponent,
    NavInfoCardComponent,
    NavFooterComponent
  ]
})
export class LeftPanelModule {}
