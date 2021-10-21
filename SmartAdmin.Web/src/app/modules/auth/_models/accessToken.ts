export interface AccessToken
{
  authenticated: boolean;
  creationDate: string;
  expiration: string;
  accessToken: string;
  refreshToken: string;
  firstAccess: boolean;
  message: string;
  userName: string;
  email: string;
}
