import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { AccessCredentials } from '@app/modules/auth/_models';


@Injectable({
  providedIn: 'root'
})
export class AuthenticationService
{

    private currentUserSubject: BehaviorSubject<AccessCredentials>;
    public currentUser: Observable<AccessCredentials>;

    constructor(private http: HttpClient)
    {
       let userCache =  localStorage.getItem('currentUser') as string;

       this.currentUserSubject = new BehaviorSubject<AccessCredentials>(JSON.parse(userCache));
       this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): AccessCredentials
    {
    	return this.currentUserSubject.value;

    }

    login(email: string, password: string, remberMe: boolean)
    {
        let accessCrendetials = new AccessCredentials(email,password,remberMe);

        return this.http.post<any>(`${environment.apiEndpoint}/usuario/Login`, {accessCrendetials })
            .pipe(map(user =>
            {
                // armazene os detalhes do usuário e o token jwt no local storage
                localStorage.setItem('currentUser', JSON.stringify(user));
                this.currentUserSubject.next(user);
                return user;
            }));
    }

    logout()
    {
        //remove os dados o usuario atual da local storage
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(new AccessCredentials('','',false));
    }

}
