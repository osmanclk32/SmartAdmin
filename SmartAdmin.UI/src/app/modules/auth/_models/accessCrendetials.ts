export class AccessCredentials
{
    constructor(Email: string,AccessKey: string,RememberMe: boolean)
    {
        this.userName = Email;
        this.email = Email  ;
        this.accessKey = AccessKey;
        this.rememberMe = RememberMe;
        this.grantType = 'password';
        this.refreshToken = undefined;
    }

    userName: string;
    email: string;
    accessKey: string;
    rememberMe: boolean;
    refreshToken?: string;
    grantType: string;
}
