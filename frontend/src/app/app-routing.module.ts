import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SalesComponent } from './components/sales/sales.component';
import { UsersComponent } from './components/users/users.component';
import { SaleDetailsComponent } from './components/sales/sale-details.component';

export const routes: Routes = [  // Adicionado 'export'
  { path: '', redirectTo: '/sales', pathMatch: 'full' },
  { path: 'sales', component: SalesComponent },
  { path: 'users', component: UsersComponent },
  { path: 'sales/:id', component: SaleDetailsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }