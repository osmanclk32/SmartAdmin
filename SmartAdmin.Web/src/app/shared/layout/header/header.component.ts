import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '@app/modules/auth/_services';

@Component({
  selector: 'sa-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

    currentUser =  this.authService.currentUserValue.userName;
    emailCurrentUser =   this.authService.currentUserValue.email;


    constructor(private authService: AuthenticationService) { }


  ngOnInit() {
  }

}
