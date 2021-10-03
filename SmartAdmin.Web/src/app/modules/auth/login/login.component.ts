import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import {Router,ActivatedRoute } from "@angular/router";
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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

    loginForm: FormGroup = new FormGroup({});
    loading = false;
    submitted = false;
    error = '';

    constructor(private formBuilder: FormBuilder,
                private route: ActivatedRoute,
                private router: Router,
                private authenticationService: AuthenticationService)
    {
        //this.loginForm = formBuilder.control

        //redirecionada para página principal caso esteja logado
        if (this.authenticationService.currentUserValue)
        {
            this.router.navigate(['/']);
        }
    }

    ngOnInit()
    {
        this.loginForm = this.formBuilder.group(
        {
            username: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

     //getter de conveniência para fácil acesso aos campos do formulário
    get frm() { return this.loginForm.controls; }

    onLogin()
    {
        this.submitted = true;

        if (this.loginForm.invalid)
        {
            return;
        }

        this.loading = true;
        this.authenticationService.login(this.frm.username.value, this.frm.password.value,this.frm.rememberMe.value)
            .pipe(first())
            .subscribe({
                next: () => {
                    // obter url de retorno dos parâmetros de rota ou padrão para '/'
                    const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
                    this.router.navigate([returnUrl]);
                },
                error: error => {
                    this.error = error;
                    this.loading = false;
                }
            });
    }

}
