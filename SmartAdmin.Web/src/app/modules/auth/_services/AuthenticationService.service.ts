import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { BehaviorSubject, EMPTY, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from '@environments/environment';
import { AccessCredentials,AccessToken,ApiResult,RefreshTokenData } from '@app/modules/auth/_models';
import { Router } from '@angular/router';


@Injectable({providedIn: 'root'})
export class AuthenticationService
{
     private currentUserSubject: BehaviorSubject<AccessToken>;
     public currentUser: Observable<AccessToken>;

     constructor(private http: HttpClient,private router: Router,)
     {
        this.currentUserSubject = new BehaviorSubject<AccessToken>(JSON.parse(localStorage.getItem('currentUser')!));
        this.currentUser = this.currentUserSubject.asObservable();
     }


     public get currentUserValue(): AccessToken
     {
        return this.currentUserSubject.value;
     }

  	 login(email: string, password: string, remberMe: boolean)
  	 {
  	    let accessCrendetials = new AccessCredentials(email,password,remberMe);

         return this.http.post<ApiResult<AccessToken>>(`${environment.apiEndpoint}/usuario/Login`, accessCrendetials )
                          			.pipe(map(user =>
                          			{
                          				// armazene os detalhes do usuário e o token jwt no local storage
                          				localStorage.setItem('currentUser', JSON.stringify(user));
                          				this.currentUserSubject.next(user.data);
                          				return user;
                          			}));

  	 }

     logout()
     {
        let nullToken = {} as AccessToken;

        //remove os dados o usuario atual da local storage
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(nullToken);
        this.router.navigate(['/login']);
     }

     refreshToken()
     {
        const accessToken = this.currentUserValue;

        if (  accessToken?.userName == undefined )
        {
            return EMPTY;
        }

        let dataRefreshToken: RefreshTokenData =
        {
            userName: accessToken.userName,
            refreshToken: accessToken.refreshToken
        };

        return this.http.post<ApiResult<AccessToken>>(`${environment.apiEndpoint}/usuario/refresh-token`, {dataRefreshToken})
            .pipe(map((user) =>
            {
               	// armazene os detalhes do usuário e o token jwt no local storage
                localStorage.setItem('currentUser', JSON.stringify(user));
                this.currentUserSubject.next(user.data);
                return user;
            }));
     }

}
