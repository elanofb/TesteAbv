import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CartsComponent } from './carts.component';

@NgModule({
  declarations: [CartsComponent],
  imports: [
    CommonModule,
    RouterModule.forChild([
      { path: '', component: CartsComponent }
    ])
  ]
})
export class CartsModule { }