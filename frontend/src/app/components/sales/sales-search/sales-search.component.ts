import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router'; 
import { SalesService } from '../../../services/sales.service';
import { Sale } from '../../../models/sale.model';

@Component({
  selector: 'app-sales-search',
  templateUrl: './sales-search.component.html',
  styleUrls: ['./sales-search.component.scss'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule]
})
export class SalesSearchComponent implements OnInit {
  searchForm: FormGroup;
  sales: Sale[] = [];

  constructor(
    private salesService: SalesService,
    private fb: FormBuilder
  ) {
    this.searchForm = this.fb.group({
      saleNumber: [''],
      startDate: [''],
      endDate: ['']
    });
  }

  ngOnInit(): void {}

  onSearch(): void {
    const formValue = this.searchForm.value;
    this.salesService.searchSales(
      formValue.saleNumber,
      formValue.startDate,
      formValue.endDate
    ).subscribe({
      next: (data: Sale[]) => {
        this.sales = data;
      },
      error: (error) => {
        console.error('Error searching sales:', error);
        this.sales = [];
      }
    });
  }
}