import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { SalesService } from '../../services/sales.service';

@Component({
  selector: 'app-sale-items-dialog',
  standalone: true,
  imports: [CommonModule, MatButtonModule, MatDialogModule],
  providers: [SalesService],  // Add this line
  template: `
    <div class="dialog-header">
      <h2>Sale Items - {{data.saleNumber}}</h2>
    </div>
    <div class="dialog-content">
      <table>
        <thead>
          <tr>
            <th>Product</th>
            <th>Quantity</th>
            <th>Unit Price</th>
            <th>Total</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let item of items">
            <td>{{item.product?.name || 'Product ' + item.productId}}</td>
            <td>{{item.quantity}}</td>
            <td>{{item.unitPrice | currency}}</td>
            <td>{{item.total | currency}}</td>
            <td>
              <button mat-button color="warn" (click)="deleteItem(item.id)">Delete</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <div class="dialog-actions">
      <button mat-button (click)="close()">Close</button>
    </div>
  `,
  styles: [`
    .dialog-header { padding: 16px; border-bottom: 1px solid #ddd; }
    .dialog-content { padding: 16px; }
    .dialog-actions { padding: 8px 16px; text-align: right; }
    table { width: 100%; border-collapse: collapse; }
    th, td { padding: 8px; text-align: left; border-bottom: 1px solid #ddd; }
    th { background-color: #f5f5f5; }
    button.mat-warn { color: #f44336; }
  `]
})
export class SaleItemsDialogComponent {
  items: any[] = [];

  constructor(
    public dialogRef: MatDialogRef<SaleItemsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private salesService: SalesService
  ) {
    console.log('Dialog Data:', data);
    this.items = Array.isArray(data) ? data : [];
  }

  deleteItem(itemId: number): void {
    if (confirm('Are you sure you want to delete this item?')) {
      this.salesService.deleteSaleItem(itemId).subscribe({
        next: () => {
          this.items = this.items.filter(item => item.id !== itemId);
          // Refresh the parent component if needed
          this.dialogRef.close({ refresh: true });
        },
        error: (error: any) => {
          console.error('Error deleting sale item:', error);
          alert('Failed to delete item. Please try again.');
        }
      });
    }
  }

  close(): void {
    this.dialogRef.close();
  }
}