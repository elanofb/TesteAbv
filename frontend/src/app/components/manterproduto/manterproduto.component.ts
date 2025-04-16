import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductsService } from '../../services/products.service';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-manterproduto',
  standalone: true,
  imports: [CommonModule, RouterModule, ReactiveFormsModule],
  templateUrl: './manterproduto.component.html',
  styleUrls: ['./manterproduto.component.css']
})
export class ManterProdutoComponent implements OnInit {
  productForm: FormGroup;
  isEditMode = false;
  productId?: number;

  constructor(
    private fb: FormBuilder,
    private productsService: ProductsService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.productForm = this.fb.group({
      name: ['', [Validators.required]],
      description: [''],
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      isAvailable: [true]
    });
  }

  ngOnInit() {
    this.productId = Number(this.route.snapshot.paramMap.get('id'));
    if (this.productId) {
      this.isEditMode = true;
      this.loadProduct(this.productId);
    }
  }

  loadProduct(id: number) {
    this.productsService.getProductById(id).subscribe({
      next: (product) => {
        this.productForm.patchValue(product);
      },
      error: (error) => {
        console.error('Error loading product:', error);
        this.router.navigate(['/users']);
      }
    });
  }

  onSubmit() {
    if (this.productForm.valid) {
      const productData = this.productForm.value;
      
      if (this.isEditMode && this.productId) {
        this.productsService.updateProduct(this.productId, productData).subscribe({
          next: () => this.router.navigate(['/users']),
          error: (error) => console.error('Error updating product:', error)
        });
      } else {
        this.productsService.createProduct(productData).subscribe({
          next: () => this.router.navigate(['/users']),
          error: (error) => console.error('Error creating product:', error)
        });
      }
    }
  }

  goBack() {
    this.router.navigate(['/users']);
  }
}