import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

//O auth guard usa o serviço de autenticação para verificar se o usuário está logado
import { AuthenticationService } from '@app/modules/auth/_services/AuthenticationService.service';

@Injectable({ providedIn: 'root'})
export class AuthGuard implements CanActivate
{

    constructor(private router: Router, private authenticationService: AuthenticationService) { }

    canActivate(route: ActivatedRouteSnapshot,   state: RouterStateSnapshot): boolean
    {
        const currentUser = this.authenticationService.currentUserValue;

        if ( currentUser )
        {
            return true;
        }

        this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });

        return false;
    }

}
