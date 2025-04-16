import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SalesService } from '../../../services/sales.service';

@Component({
  selector: 'app-sale-items-dialog',
  templateUrl: './sale-items-dialog.component.html',
  styleUrls: ['./sale-items-dialog.component.css']
})
export class SaleItemsDialogComponent {
  saleItems: any[] = [];
  displayedColumns: string[] = ['product', 'quantity', 'price', 'total'];

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: { saleId: number },
    private salesService: SalesService
  ) {
    this.loadSaleItems();
  }

  private loadSaleItems() {
    this.salesService.getSaleItems(this.data.saleId).subscribe(
      items => this.saleItems = items
    );
  }
}