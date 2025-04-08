import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterModule, NavMenuComponent],
  template: `
    <app-nav-menu></app-nav-menu>
    <router-outlet></router-outlet>
  `
})
export class AppComponent {
  title = 'DeveloperStore';
}
