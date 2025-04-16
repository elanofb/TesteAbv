// ... existing imports ...
import { MatDialog } from '@angular/material/dialog';
import { SaleItemsDialogComponent } from './sale-items-dialog/sale-items-dialog.component';

export class SalesComponent {
  // ... existing properties ...
  displayedColumns: string[] = [...this.displayedColumns, 'actions'];

  constructor(
    private dialog: MatDialog,
    // ... other existing dependencies ...
  ) {}

  viewSaleItems(sale: any) {
    this.dialog.open(SaleItemsDialogComponent, {
      width: '600px',
      data: { saleId: sale.id }
    });
  }
  // ... existing code ...
}