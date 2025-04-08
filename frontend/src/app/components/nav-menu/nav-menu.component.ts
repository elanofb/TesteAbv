import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-nav-menu',
  standalone: true,
  imports: [RouterModule, CommonModule],
  template: `
    <nav class="navbar">
      <a routerLink="/sales" routerLinkActive="active">Sales</a>
      <a routerLink="/carts" routerLinkActive="active">Carts</a>
      <a routerLink="/users" routerLinkActive="active">Users</a>
    </nav>
  `,
  styles: [`
    .navbar {
      background: #333;
      padding: 1rem;
      display: flex;
      gap: 2rem;
    }
    a {
      color: white;
      text-decoration: none;
      padding: 0.5rem 1rem;
    }
    a:hover {
      background: #444;
    }
    .active {
      background: #555;
    }
  `]
})
export class NavMenuComponent {}