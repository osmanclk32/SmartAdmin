import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { AuthenticationService } from '@app/modules/auth/_services/AuthenticationService.service';

//Error Interceptor intercepta as respostas http da api para verificar se houve algum erro.
//Se a resposta for 401 Unauthorizedou 403 Forbidden o usuário é desconectado automaticamente do aplicativo, todos os outros erros serão registrados no console e enviados novamente ao serviço de chamada para que um alerta com o erro possa ser exibido na IU.
@Injectable()
export class ErrorInterceptor implements HttpInterceptor
{
    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
    {
        return next.handle(request).pipe(catchError(err =>
        {
            if ([401, 403].includes(err.status) && this.authenticationService.currentUserValue)
            {
                // auto logout se 401 ou 403 for retornado da api
                this.authenticationService.logout();
            }

            const error = (err && err.error && err.error.message) || err.statusText;
            console.error(err);
            return throwError(error);
        }))
    }
}
