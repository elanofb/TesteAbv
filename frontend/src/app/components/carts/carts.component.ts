import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-carts',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="carts-container">
      <h2>Carts Management</h2>
      <p>Welcome to the Carts page</p>
    </div>
  `,
  styles: [`
    .carts-container {
      padding: 20px;
      margin: 20px;
    }
  `]
})
export class CartsComponent {}