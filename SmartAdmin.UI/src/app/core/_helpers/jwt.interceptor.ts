import { Injectable } from '@angular/core';
import { HttpRequest, HttpHandler, HttpEvent, HttpInterceptor } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '@environments/environment';
import { AuthenticationService } from '@app/modules/auth/_services';

@Injectable()
export class JwtInterceptor implements HttpInterceptor
{
    constructor(private authenticationService: AuthenticationService) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
    {
        // adicione o cabeçalho de autenticação com jwt se o usuário estiver conectado e a solicitação for para a URL da API
        const currentUser = this.authenticationService.currentUserValue;
        const isLoggedIn = currentUser && currentUser.accessToken ;
        const isApiUrl = request.url.startsWith(environment.apiEndpoint);

        if (isLoggedIn && isApiUrl)
        {
            request = request.clone(
            {
                setHeaders:
                {
                    Authorization: `Bearer ${currentUser.accessToken}`
                }
            });
        }

        return next.handle(request);
    }
}
