import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { SalesService } from '../../services/sales.service';
import { Sale } from '../../models/sale.model';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { SaleItemsDialogComponent } from './sale-items-dialog.component';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sales',
  templateUrl: './sales.component.html',
  styleUrls: ['./sales.component.scss'],
  standalone: true,
  imports: [
    CommonModule, 
    ReactiveFormsModule,
    MatDialogModule,
    RouterModule
  ]
})
export class SalesComponent implements OnInit {
  sales: Sale[] = [];
  saleForm: FormGroup;
  editMode = false;
  currentSaleId: number | null = null;

  constructor(
    private salesService: SalesService,
    private fb: FormBuilder,
    private dialog: MatDialog
  ) {
    this.saleForm = this.fb.group({
      saleNumber: ['', Validators.required],
      customer: ['', Validators.required],
      saleDate: ['', Validators.required],
      productId: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
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

  onSubmit() {
    if (this.saleForm.valid) {
      const formValue = this.saleForm.value;
      
      const itemTotal = Number(formValue.quantity) * Number(formValue.unitPrice);
      const discountAmount = (Number(formValue.discount) || 0) / 100 * itemTotal;
      const finalItemTotal = itemTotal - discountAmount;
  
      const saleData = {
        id: 0,
        saleNumber: formValue.saleNumber,
        saleDate: new Date(formValue.saleDate).toISOString(),
        customer: formValue.customer,
        totalAmount: finalItemTotal,
        branch: formValue.branch,
        items: [{
          productId: formValue.productId,
          quantity: formValue.quantity,
          unitPrice: formValue.unitPrice,
          discount: formValue.discount || 0,
          total: finalItemTotal
        }]
      };
  
      console.log('Submitting sale:', saleData);
  
      this.salesService.createSale(saleData).subscribe({
        next: (response) => {
          console.log('Sale created successfully:', response);
          this.loadSales();
          this.saleForm.reset();
        },
        error: (error) => {
          console.error('Error creating sale:', error);
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

  viewItems(sale: any) {
    this.salesService.getSaleItems(sale.id).subscribe({
      next: (items) => {
        console.log('Sale Items:', items);
        this.dialog.open(SaleItemsDialogComponent, {
          width: '600px',
          data: items
        });
      },
      error: (error) => {
        console.error('Error fetching sale items:', error);
      }
    });
  }
}
