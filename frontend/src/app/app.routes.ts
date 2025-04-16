import { Routes } from '@angular/router';
import { SalesComponent } from './components/sales/sales.component';
import { SalesSearchComponent } from './components/sales/sales-search/sales-search.component';
import { CartsComponent } from './components/carts/carts.component';
import { ProductsListComponent } from './components/products/products-list.component';
import { ProductFormComponent } from './components/products/product-form.component';
import { ProdutosComponent } from './components/produtos/produtos.component';
import { ManterProdutoComponent } from './components/manterproduto/manterproduto.component';

export const routes: Routes = [
  { path: '', redirectTo: 'sales/create', pathMatch: 'full' },
  { path: 'carts', component: CartsComponent},
  { path: 'products', component: ProductsListComponent, pathMatch: 'full'},
  { path: 'products/new', component: ProductFormComponent },
  { path: 'products/:id', component: ProductFormComponent },
  { path: 'produtos', component: ProdutosComponent },
  { path: 'manterproduto', component: ManterProdutoComponent },
  { path: 'manterproduto/:id', component: ManterProdutoComponent },
  // {
  //   path: 'products',
  //   loadChildren: () => import('./components/products/products.module').then(m => m.ProductsModule)
  // },
  {
    path: 'sales',
    children: [
      { path: '', redirectTo: 'create', pathMatch: 'full' },
      { path: 'create', component: SalesComponent },
      { path: 'search', component: SalesSearchComponent },      
    ]
  }
];
