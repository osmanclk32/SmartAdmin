import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import {Router} from "@angular/router";

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: [
               './login.component.css',
               '../../../../assets/css/fa-brands.css'],
               encapsulation: ViewEncapsulation.None
})
export class LoginComponent implements OnInit {

  constructor(private router: Router) { }

  ngOnInit() {
  }

  login(event: any)
  {
    event.preventDefault();
    //this.router.navigate(['/dashboard/+analytics'])
  }

}
