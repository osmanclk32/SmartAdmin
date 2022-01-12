//Modulos Angular
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule,HTTP_INTERCEPTORS } from '@angular/common/http';


//Modulos da externos
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
//import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

//Modulos da aplicação
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthRoutes } from './modules/auth/auth.routing';
import {SmartAdminLayoutModule} from "./shared/layout.module";
import { ErrorInterceptor } from './core/_helpers/error.interceptor';
import { JwtInterceptor } from './core/_helpers/jwt.interceptor';
import { AuthenticationService } from './modules/auth/_services';
import { appInitializer } from './core/app.initializer';



@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    SmartAdminLayoutModule,
    AuthRoutes,
    HttpClientModule,
    SweetAlert2Module.forRoot(),
    NgbModule,
    CommonModule,
    BrowserAnimationsModule, // required animations module
    ToastrModule.forRoot({
    timeOut: 10000,
    positionClass: 'toast-top-right',
    preventDuplicates: true,
    enableHtml: true,
    progressBar: true,
    easing: 'ease-in',
  }), // ToastrModule added
  ],
  providers:
  [
      { provide: APP_INITIALIZER, useFactory: appInitializer, multi: true, deps: [AuthenticationService] },
      { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true  },
      { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
