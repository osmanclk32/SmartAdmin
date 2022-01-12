
import {NgModule} from "@angular/core";
import {CommonModule} from "@angular/common";
import {RouterModule} from "@angular/router";
import {FormsModule} from "@angular/forms";

import { MainLayoutComponent } from './layout/app-layouts/main-layout/main-layout.component';
import { AuthLayoutComponent } from "./layout/app-layouts/main-layout/auth-layout.component";
import { NavigationModule } from "./layout/navigation/navigation.module";
import { LeftPanelModule } from "./layout/navigation/left-panel/left-panel.module";
import { HeaderModule } from "./layout/header/header.module";
import { LogoModule } from "./layout/logo/logo.module";
import { PageFooterComponent } from './layout/page-footer/page-footer.component';
import { PageFooterModule } from "./layout/page-footer/page-footer.module";



@NgModule({
  imports: [
    CommonModule,
     RouterModule,
     LeftPanelModule,
     NavigationModule,
     LogoModule,
     HeaderModule,
     PageFooterModule
  ],
  declarations: [
    MainLayoutComponent,
    AuthLayoutComponent
  ],
  exports:[
    NavigationModule,
    LeftPanelModule,
    LogoModule,
    HeaderModule,
    PageFooterComponent
  ]
})
export class SmartAdminLayoutModule {}
