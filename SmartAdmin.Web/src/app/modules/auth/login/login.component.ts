import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import {Router,ActivatedRoute } from "@angular/router";
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';

import { AuthenticationService } from '@app/modules/auth/_services';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: [
               './login.component.css',
               '../../../../assets/css/fa-brands.css'],
               encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit
{

    loginForm!: FormGroup;
    loading = false;
    submitted = false;
    isLoginFailed =false;
    isLoggedIn = false;

    falhaLoginMsg = '';

    constructor(private formBuilder: FormBuilder,
                private route: ActivatedRoute,
                private router: Router,
                private authenticationService: AuthenticationService)
    {

        //redirecionada para página principal caso esteja logado
        if (this.authenticationService.currentUserValue?.accessToken != undefined  )
        {
             this.isLoggedIn = true;

            this.router.navigate(['/']);

        }
    }

    ngOnInit()
    {
        this.loginForm = this.formBuilder.group(
        {
            username: ['', Validators.required],
            password: ['', Validators.compose([Validators.required,Validators.minLength(4)])],
            rememberMe: ['']
        });
    }

     //getter de conveniência para fácil acesso aos campos do formulário
    get frm() { return this.loginForm.controls; }

    onLogin()
    {
        this.submitted = true;

        this.loading = true;

        let remember = this.frm.rememberMe?.value == '' ? false : this.frm.rememberMe.value;

        this.authenticationService.login(this.frm.username.value, this.frm.password.value,remember)
            .pipe(first())
            .subscribe(
            {
                next: () =>
                {
                    // obter url de retorno dos parâmetros de rota ou padrão para '/'
                    const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

                    this.isLoggedIn = true;

                    this.router.navigate([returnUrl]);
                }
            });
    }

}
