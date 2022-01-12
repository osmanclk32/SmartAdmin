//Modulos Angular
import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule,HTTP_INTERCEPTORS } from '@angular/common/http';

//Modulos da externos
//import { NgbModule } from '@ngx-bootstrap/ng-bootstrap';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

//Modulos da aplicação
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthRoutes } from './modules/auth/auth.routing';
import { LayoutModule } from "./layout/layout.module";
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
    LayoutModule,
    AuthRoutes,
    HttpClientModule,
    SweetAlert2Module.forRoot(),
    ToastrModule.forRoot({
    timeOut: 10000,
    positionClass: 'toast-top-right',
    preventDuplicates: true,
    enableHtml: true,
    progressBar: true,
    easing: 'ease-in',
  })
  ],
  providers:
  [
      { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
      { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true  },
      { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
