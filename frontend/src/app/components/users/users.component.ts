import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductsService } from '../../services/products.service';
import { Product } from '../../models/product.model';  // Update import path if needed

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, RouterModule, MatButtonModule, ReactiveFormsModule],
  template: `
    <div class="container">
      <h2>Products Management</h2>
      
      <!-- Form Section -->
      <div class="form-section">
        <h3>{{isEditMode ? 'Edit' : 'Add New'}} Product</h3>
        <form [formGroup]="productForm" (ngSubmit)="onSubmit()">
          <div class="form-group">
            <label>Name:</label>
            <input type="text" formControlName="name">
            <div class="error" *ngIf="productForm.get('name')?.errors?.['required'] && productForm.get('name')?.touched">
              Name is required
            </div>
          </div>

          <div class="form-group">
            <label>Description:</label>
            <textarea formControlName="description" rows="3"></textarea>
          </div>

          <div class="form-group">
            <label>Unit Price:</label>
            <input type="number" formControlName="unitPrice" step="0.01">
            <div class="error" *ngIf="productForm.get('unitPrice')?.errors?.['required'] && productForm.get('unitPrice')?.touched">
              Price is required
            </div>
          </div>

          <div class="form-group checkbox">
            <label>
              <input type="checkbox" formControlName="isAvailable">
              Available
            </label>
          </div>

          <div class="form-actions">
            <button type="submit" class="btn-save" [disabled]="!productForm.valid">
              {{isEditMode ? 'Update' : 'Save'}}
            </button>
            <button type="button" class="btn-cancel" *ngIf="isEditMode" (click)="cancelEdit()">
              Cancel
            </button>
          </div>
        </form>
      </div>

      <!-- List Section -->
      <div class="list-section">
        <h3>Products List</h3>
        <table *ngIf="products.length > 0">
          <thead>
            <tr>
              <th>Name</th>
              <th>Description</th>
              <th>Unit Price</th>
              <th>Status</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            <tr *ngFor="let product of products">
              <td>{{product.name}}</td>
              <td>{{product.description}}</td>
              <td>{{product.price | currency}}</td>  // Changed from unitPrice to price
              <td>{{product.isAvailable ? 'Available' : 'Not Available'}}</td>
              <td>
                <button mat-button (click)="editProduct(product)">Edit</button>
                <button mat-button color="warn" (click)="deleteProduct(0)">Delete</button>
              </td>
            </tr>
          </tbody>
        </table>
        
        <div *ngIf="products.length === 0" class="no-data">
          No products found
        </div>
      </div>
    </div>
  `,
  styles: [`
    .container { padding: 20px; }
    .form-section { 
      background: #f9f9f9;
      padding: 20px;
      border-radius: 8px;
      margin-bottom: 30px;
    }
    .form-group { 
      margin-bottom: 15px;
    }
    .form-group label {
      display: block;
      margin-bottom: 5px;
    }
    input[type="text"],
    input[type="number"],
    textarea {
      width: 100%;
      padding: 8px;
      border: 1px solid #ddd;
      border-radius: 4px;
    }
    .error {
      color: red;
      font-size: 12px;
      margin-top: 4px;
    }
    .form-actions {
      margin-top: 20px;
    }
    button {
      padding: 8px 16px;
      margin-right: 10px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
    }
    .btn-save {
      background-color: #4CAF50;
      color: white;
    }
    .btn-cancel {
      background-color: #f44336;
      color: white;
    }
    table { 
      width: 100%;
      border-collapse: collapse;
      margin-top: 20px;
    }
    th, td { 
      padding: 8px;
      text-align: left;
      border-bottom: 1px solid #ddd;
    }
    th { background-color: #f5f5f5; }
    .no-data { 
      text-align: center;
      padding: 20px;
      color: #666;
    }
  `]
})
export class UsersComponent implements OnInit {
  products: Product[] = [];
  productForm: FormGroup;
  isEditMode = false;
  editingProductId?: number;

  constructor(
    private productsService: ProductsService,
    private fb: FormBuilder
  ) {
    this.productForm = this.fb.group({
      name: ['', [Validators.required]],
      description: [''],
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      isAvailable: [true]
    });
  }

  ngOnInit() {
    this.loadProducts();
  }

  loadProducts() {
    this.productsService.getAllProducts().subscribe({
      next: (products) => {
        console.log('Products loaded:', products);
        this.products = products;
      },
      error: (error) => {
        console.error('Error loading products:', error);
        this.products = [];
      }
    });
  }

  onSubmit() {
    if (this.productForm.valid) {
      const productData: Product = {
        name: this.productForm.value.name,
        description: this.productForm.value.description,
        price: Number(this.productForm.value.unitPrice),  // Changed to price
        isAvailable: this.productForm.value.isAvailable
      };
      
      if (this.isEditMode && this.editingProductId) {
        productData.id = this.editingProductId;  // Changed this line
        this.productsService.updateProduct(this.editingProductId, productData).subscribe({
          next: () => {
            this.loadProducts();
            this.resetForm();
          },
          error: (error) => console.error('Error updating product:', error)
        });
      } else {
        this.productsService.createProduct(productData).subscribe({
          next: () => {
            this.loadProducts();
            this.resetForm();
          },
          error: (error) => console.error('Error creating product:', error)
        });
      }
    }
  }

  editProduct(product: any) {
    this.isEditMode = true;
    this.editingProductId = product.id;
    this.productForm.patchValue(product);
  }

  cancelEdit() {
    this.resetForm();
  }

  resetForm() {
    this.isEditMode = false;
    this.editingProductId = undefined;
    this.productForm.reset({
      name: '',
      description: '',
      unitPrice: 0,
      isAvailable: true
    });
  }

  deleteProduct(id: number) {
    if (confirm('Are you sure you want to delete this product?')) {
      this.productsService.deleteProduct(id).subscribe({
        next: () => {
          this.loadProducts();
        },
        error: (error) => console.error('Error deleting product:', error)
      });
    }
  }
}
