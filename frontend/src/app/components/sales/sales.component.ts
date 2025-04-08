import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SalesService } from '../../services/sales.service';
import { Sale } from '../../models/sale.model';

@Component({
  selector: 'app-sales',
  templateUrl: './sales.component.html',
  styleUrls: ['./sales.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class SalesComponent implements OnInit {
  sales: Sale[] = [];
  saleForm: FormGroup;
  editMode = false;
  currentSaleId: number | null = null;

  constructor(
    private salesService: SalesService,
    private fb: FormBuilder
  ) {
    this.saleForm = this.fb.group({
      saleNumber: ['', Validators.required],
      customer: ['', Validators.required],
      saleDate: [new Date().toISOString().split('T')[0], Validators.required],
      productId: [0, [Validators.required, Validators.min(0)]],
      quantity: [1, [Validators.required, Validators.min(1)]], // Mínimo de 1
      unitPrice: [0, [Validators.required, Validators.min(0)]],
      discount: [0, [Validators.min(0), Validators.max(100)]], // Opcional, entre 0 e 100
      branch: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadSales();
  }

  loadSales(): void {
    this.salesService.getAllSales().subscribe({
      next: (data: Sale[]) => {
        console.log('Sales data received:', data); // Debug
        this.sales = data || [];
      },
      error: (error) => {
        console.error('Error details:', error); // Debug detalhado
        if (error.status === 0) {
          console.warn('Backend connection failed. Please check if the API is running.');
        }
        this.sales = [];
      }
    });
  }

  onSubmit(): void {
    if (this.saleForm.valid) {
      const formValue = this.saleForm.value;
      
      const itemTotal = Number(formValue.quantity) * Number(formValue.unitPrice);
      const discountAmount = (Number(formValue.discount) || 0) / 100 * itemTotal;
      const finalItemTotal = itemTotal - discountAmount;
  
      const saleData = {
        saleNumber: formValue.saleNumber,
        saleDate: new Date(formValue.saleDate).toISOString(),
        customer: formValue.customer,
        totalAmount: finalItemTotal,
        branch: formValue.branch,
        items: [
          {
            productId: Number(formValue.productId),
            quantity: Number(formValue.quantity),
            unitPrice: Number(formValue.unitPrice),
            discount: Number(formValue.discount || 0),
            total: finalItemTotal
          }
        ]
      };
  
      console.log('Sending sale data:', saleData);
  
      this.salesService.createSale(saleData).subscribe({
        next: (response) => {
          console.log('Sale created successfully:', response);
          this.loadSales();
          this.resetForm();
        },
        error: (error) => {
          console.error('Error creating sale:', error);
          alert(`Error creating sale: ${error.error?.message || 'Unknown error'}`);
        }
      });
    }
  }

  editSale(sale: Sale): void {
    this.editMode = true;
    this.currentSaleId = sale.id || null;
    this.saleForm.patchValue({
      saleNumber: sale.saleNumber,
      customer: sale.customer,
      saleDate: new Date(sale.saleDate).toISOString().split('T')[0],
      totalAmount: sale.totalAmount,
      branch: sale.branch
    });
  }

  deleteSale(id: number): void {
    if (confirm('Are you sure you want to delete this sale?')) {
      this.salesService.deleteSale(id).subscribe({
        next: () => this.loadSales(),
        error: (error: Error) => console.error('Error deleting sale:', error)
      });
    }
  }

  cancelEdit(): void {
    this.resetForm();
  }

  private resetForm(): void {
    this.saleForm.reset();
    this.editMode = false;
    this.currentSaleId = null;
  }
}
