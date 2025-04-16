import { Component } from '@angular/core';

@Component({
  selector: 'app-products-list',
  // remover a linha standalone: true
  template: `
    <div>
      <h1>Products Page</h1>
      <p>This is a test page</p>
    </div>
  `
})
export class ProductsListComponent {
  constructor() {
    console.log('ProductsListComponent initialized');
  }
}