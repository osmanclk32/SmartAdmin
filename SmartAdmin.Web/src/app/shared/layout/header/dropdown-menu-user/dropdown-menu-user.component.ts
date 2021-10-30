import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '@app/modules/auth/_services';

@Component({
  selector: 'sa-dropdown-menu-user',
  templateUrl: './dropdown-menu-user.component.html',
  styleUrls: ['./dropdown-menu-user.component.css']
})
export class DropdownMenuUserComponent implements OnInit
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
