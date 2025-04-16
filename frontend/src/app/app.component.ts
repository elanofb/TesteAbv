import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { ProductsListComponent } from './components/products/products-list.component';
import { HeaderComponent } from './components/shared/header.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, NavMenuComponent, ProductsListComponent, HeaderComponent],
  template: `
    <app-header></app-header>
    <app-nav-menu></app-nav-menu>
    <router-outlet></router-outlet>
  `
})
export class AppComponent {
  title = 'DeveloperStore';
}
