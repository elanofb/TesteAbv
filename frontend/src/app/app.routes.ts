import { Routes } from '@angular/router';
import { SalesComponent } from './components/sales/sales.component';
import { SalesSearchComponent } from './components/sales/sales-search/sales-search.component';
import { CartsComponent } from './components/carts/carts.component';

export const routes: Routes = [
  { path: '', redirectTo: 'sales/create', pathMatch: 'full' },
  { path: 'carts', component: CartsComponent },
  {
    path: 'sales',
    children: [
      { path: '', redirectTo: 'create', pathMatch: 'full' },
      { path: 'create', component: SalesComponent },
      { path: 'search', component: SalesSearchComponent }
    ]
  }
];
