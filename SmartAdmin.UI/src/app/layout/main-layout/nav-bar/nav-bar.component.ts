import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '@app/modules/auth/_services';

@Component({
  selector: 'sa-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit
{

    currentUser =  this.authService.currentUserValue.userName;
    emailCurrentUser =   this.authService.currentUserValue.email;


    constructor(private authService: AuthenticationService) { }

    ngOnInit() {}

    logout()
    {
        this.authService.logout();
    }



}
