import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-nav-menu',
  standalone: true,
  imports: [RouterModule, CommonModule],
  template: `
    <nav>
      <ul>
        <li><a routerLink="/sales" routerLinkActive="active">Sales</a></li>
        <!-- <li><a routerLink="/products" routerLinkActive="active">Products</a></li>
        <li><a routerLink="/produtos" routerLinkActive="active">Produtos</a></li> -->
        <li><a routerLink="/users" routerLinkActive="active">Produtos</a></li>
      </ul>
    </nav>
  `,
  styles: [`
    nav {
      background-color: #333;
      padding: 1rem;
    }
    ul {
      list-style: none;
      margin: 0;
      padding: 0;
      display: flex;
      gap: 1rem;
    }
    a {
      color: white;
      text-decoration: none;
      padding: 0.5rem 1rem;
    }
    a:hover, a.active {
      background-color: #555;
      border-radius: 4px;
    }
  `]
})
export class NavMenuComponent {}