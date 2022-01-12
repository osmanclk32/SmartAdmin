import { Component, OnInit } from '@angular/core';
import { SmartNavigationMenu } from '@app/modules/auth/_models/SmartNavigationMenu';
import { AuthenticationService } from '@app/modules/auth/_services';
import { UserService } from '@app/modules/auth/_services/User.service';

@Component({
  selector: 'sa-main-side-bar',
  templateUrl: './main-side-bar.component.html',
  styleUrls: ['./main-side-bar.component.scss']
})
export class MainSideBarComponent implements OnInit
{

    currentUser =  this.authService.currentUserValue.userName;

    itensMenu: SmartNavigationMenu[] = [];

    constructor(private authService: AuthenticationService,private userService: UserService) { }

  ngOnInit()
  {
      this.userService.listaItensMenu(1,1).subscribe(response =>
      {
         this.itensMenu = response;
      },
      error =>
      {
         console.log(error);
      })
  }

}
