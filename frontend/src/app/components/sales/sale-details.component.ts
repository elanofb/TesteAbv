import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { SalesService } from '../../services/sales.service';
import { MatButtonModule } from '@angular/material/button';
import { ProductsService } from '../../services/products.service';

@Component({
  selector: 'app-sale-details',
  standalone: true,
  imports: [CommonModule, MatButtonModule, ReactiveFormsModule],
  template: `
    <div class="details-container">
      <h2>Sale Details</h2>
      <div class="sale-info" *ngIf="sale">
        <div class="info-group">
          <label>Sale Number:</label>
          <span>{{sale.saleNumber}}</span>
        </div>
        <div class="info-group">
          <label>Customer:</label>
          <span>{{sale.customer}}</span>
        </div>
        <div class="info-group">
          <label>Date:</label>
          <span>{{sale.saleDate | date:'shortDate'}}</span>
        </div>
        <div class="info-group">
          <label>Branch:</label>
          <span>{{sale.branch}}</span>
        </div>
        <div class="info-group">
          <label>Total Amount:</label>
          <span>{{sale.totalAmount | currency}}</span>
        </div>
      </div>

      <!-- New Product Form -->
      <div class="add-item-section">
        <h3>Add New Product</h3>
        <form [formGroup]="itemForm" (ngSubmit)="addItem()">        
        <div class="form-group">
          <select formControlName="productId" class="form-control">
            <option value="">Select a product</option>
            <option *ngFor="let product of products" [value]="product.id">
              {{product.name}} - {{product.unitPrice | currency}}
            </option>
          </select>
        </div>
          <div class="form-group">
            <input type="number" formControlName="quantity" placeholder="Quantity">
          </div>
          <div class="form-group">
            <input type="number" formControlName="unitPrice" placeholder="Unit Price">
          </div>
          <div class="form-group">
            <input type="number" formControlName="discount" placeholder="Discount (%)">
          </div>
          <button type="submit" [disabled]="!itemForm.valid">Add Product</button>
        </form>
      </div>

      <div class="items-section">
        <h3>Items</h3>
        <table>
          <thead>
            <tr>
              <th>Product</th>
              <th>Quantity</th>
              <th>Unit Price</th>
              <th>Discount</th>
              <th>Total</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let item of saleItems">
              <td>{{getProductName(item.productId)}}</td>
              <td>{{item.quantity}}</td>
              <td>{{item.unitPrice | currency}}</td>
              <td>{{item.discount}}%</td>
              <td>{{item.total | currency}}</td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="actions">
        <button mat-button (click)="goBack()">Back to Sales</button>
      </div>
    </div>
  `,
  styles: [`
    .details-container { padding: 20px; max-width: 1200px; margin: 0 auto; }
    .sale-info { margin: 20px 0; }
    .info-group { margin: 10px 0; }
    .info-group label { font-weight: bold; margin-right: 10px; }
    table { width: 100%; border-collapse: collapse; margin: 20px 0; }
    th, td { padding: 8px; text-align: left; border-bottom: 1px solid #ddd; }
    th { background-color: #f5f5f5; }
    .actions { margin-top: 20px; }
    .add-item-section { 
      margin: 20px 0;
      padding: 20px;
      background-color: #f9f9f9;
      border-radius: 4px;
    }
    .form-group {
      margin: 10px 0;
    }
    .form-group input {
      padding: 8px;
      width: 200px;
      margin-right: 10px;
      border: 1px solid #ddd;
      border-radius: 4px;
      height: 30px;
    }
    select.form-control {
        padding: 8px;
        width: 200px;
        margin-right: 10px;
        border: 1px solid #ddd;
        border-radius: 4px;
        height: 35px;
    }
    button[type="submit"] {
      padding: 8px 16px;
      background-color: #4CAF50;
      color: white;
      border: none;
      border-radius: 4px;
      cursor: pointer;
    }
    button[type="submit"]:disabled {
      background-color: #cccccc;
      cursor: not-allowed;
    }
  `]
})
export class SaleDetailsComponent implements OnInit {
  sale: any;
  saleItems: any[] = [];
  itemForm: FormGroup;
  products: any[] = []; // Add this line

  constructor(
    private route: ActivatedRoute,
    private salesService: SalesService,
    private productsService: ProductsService,
    private fb: FormBuilder
  ) {
    this.itemForm = this.fb.group({
      productId: ['', [Validators.required]],
      quantity: [1, [Validators.required, Validators.min(1)]],
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      discount: [0, [Validators.min(0), Validators.max(100)]]
    });
  }

  ngOnInit() {
    const id = this.route.snapshot.params['id'];
    this.loadSaleDetails(id);
    this.loadProducts(); // Add this line
  }

  getProductName(productId: number): string {
    const product = this.products.find(p => p.id === productId);
    return product ? product.name : 'Product ' + productId;
  }
//   loadProducts() {
//     this.salesService.getProducts().subscribe({
//       next: (products) => {
//         this.products = products;
//       },
//       error: (error) => console.error('Error loading products:', error)
//     });
//   }
    loadProducts() {
        this.productsService.getAllProducts().subscribe({
        next: (products) => {
            console.log('Products loaded:', products);
            this.products = products;
        },
        error: (error) => console.error('Error loading products:', error)
        });
    }

    loadSaleDetails(id: number) {
    this.salesService.getSaleById(id).subscribe({
        next: (sale) => {
        this.sale = sale;
        this.loadSaleItems(id);
        },
        error: (error) => console.error('Error loading sale:', error)
    });
    }

    loadSaleItems(saleId: number) {
      this.salesService.getSaleItems(saleId).subscribe({
        next: (items) => {
          // Map each item to include the product details
          this.saleItems = items.map(item => {
            const product = this.products.find(p => p.id === item.productId);
            return {
              ...item,
              product: product
            };
          });
        },
        error: (error) => console.error('Error loading items:', error)
      });
    }

    goBack() {
    window.history.back();
    }

  addItem() {
    if (this.itemForm.valid && this.sale) {
      const formValues = this.itemForm.value;
      const saleId = this.route.snapshot.params['id'];
      
      const newItem = {
        "saleId": saleId,
        "productId": formValues.productId,
        "quantity": formValues.quantity,
        "unitPrice": formValues.unitPrice,
        "discount": formValues.discount || 0
      };
  
      console.log('Sending payload:', newItem);
    
      this.salesService.addSaleItem(newItem).subscribe({
        next: (response) => {
          console.log('Item added successfully:', response);
          this.loadSaleItems(saleId);
          this.itemForm.reset({
            productId: '',
            quantity: 1,
            unitPrice: 0,
            discount: 0
          });
        },
        error: (error) => {
          console.error('Error adding item:', error);
          alert('Failed to add item. Please try again.');
        }
      });
    }
  }
}