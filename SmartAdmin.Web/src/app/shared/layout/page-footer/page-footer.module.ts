import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageFooterComponent } from './page-footer.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [PageFooterComponent],
   exports: [PageFooterComponent]
})
export class PageFooterModule { }
