import { UserService } from './../../../../../modules/auth/_services/User.service';
import { SmartNavigationMenu } from '../../../../../modules/auth/_models/SmartNavigationMenu';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'sa-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit
{

   itensMenu: SmartNavigationMenu[] = [];

  constructor(private userService: UserService) { }

  ngOnInit()
  {
      this.userService.listaItensMenu(1,1).subscribe(response =>
      {
         this.itensMenu = response
      },
      error =>
      {
         console.log(error);
      })
  }

}
