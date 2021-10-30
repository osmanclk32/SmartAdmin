import { Injectable } from '@angular/core';
import {HttpErrorResponse, HttpResponse, HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';

import { throwError, Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { AuthenticationService } from '@app/modules/auth/_services/AuthenticationService.service';
import { environment } from '@environments/environment';
import Swal from 'sweetalert2';
import { ToastrService } from 'ngx-toastr';
import HttpStatusCode from '@app/core/_models/http-status-code'

//Error Interceptor intercepta as respostas http da api para verificar se houve algum erro.
//Se a resposta for 401 Unauthorizedou 403 Forbidden o usuário é desconectado automaticamente do aplicativo, todos os outros erros serão registrados no console e enviados novamente ao serviço de chamada para que um alerta com o erro possa ser exibido na IU.
@Injectable()
export class ErrorInterceptor implements HttpInterceptor
{
    constructor(private authenticationService: AuthenticationService,private notify: ToastrService ) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
    {
        return next.handle(request).pipe(map((event: HttpEvent<any>) =>
        {
            if (event instanceof HttpResponse)
            {
                if (event.status === HttpStatusCode.NO_CONTENT)
                {
                    this.notify.warning('Nenhum registro localizado.', 'Atenção');
                }
            }
            return event;
        }),
        catchError((err: HttpErrorResponse) =>
        {
            const { error } = err;

            if ([401, 403].includes(err.status) && this.authenticationService.currentUserValue)
            {
                // auto logout se 401 ou 403 for retornado da api
                this.authenticationService.logout();
            }

            if (err.status)
            {
               if (this.isServerError(err)) this.notify.error(error.message, 'Atenção');
               else this.notify.warning(error.message, 'Atenção');
            }
            else
            {
               //this.notify.error('Erro ao se conectar com o servidor.', 'Atenção');
               Swal.fire({
                        title: 'Erro',
                        text: 'Erro ao se conectar com o servidor.',
                        icon: 'error',
                        showCancelButton: false
                      });
            }

            return throwError(err);
        }));


    }

    private isServerError(error: any): boolean
    {
        return error.status === HttpStatusCode.INTERNAL_SERVER_ERROR;
    }
}
