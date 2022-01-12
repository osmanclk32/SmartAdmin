import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'sa-page-footer',
  templateUrl: './page-footer.component.html',
  styleUrls: ['./page-footer.component.css']
})

export class PageFooterComponent implements OnInit
{

    anoCorrente: number = 0;
    copyRight: string = '';

    constructor() { }

    ngOnInit()
    {
        this.anoCorrente = (new Date()).getFullYear();
        this.copyRight =  `${this.anoCorrente} © SmartAdmin by Siltec Software`;
    }

}
