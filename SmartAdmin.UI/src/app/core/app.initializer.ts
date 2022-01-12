import { AuthenticationService } from '@app/modules/auth/_services';

export function appInitializer(authenticationService: AuthenticationService)
{
    return () => new Promise(resolve =>
    {
        // tentativa de atualizar o token na inicialização do aplicativo para autenticação automática
        authenticationService.refreshToken()
            .subscribe();
    });
}
