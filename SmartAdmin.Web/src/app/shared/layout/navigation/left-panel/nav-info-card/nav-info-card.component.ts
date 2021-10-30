import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '@app/modules/auth/_services';

@Component({
  selector: 'sa-nav-info-card',
  templateUrl: './nav-info-card.component.html',
  styleUrls: ['./nav-info-card.component.css']
})
export class NavInfoCardComponent implements OnInit
{

    currentUser =  this.authService.currentUserValue.userName;

    constructor(private authService: AuthenticationService) { }

    ngOnInit()
    {

    }

}
