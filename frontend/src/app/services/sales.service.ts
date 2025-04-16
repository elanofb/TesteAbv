import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map, catchError } from 'rxjs';
import { Sale } from '../models/sale.model';

@Injectable({
  providedIn: 'root'
})
export class SalesService {
  private apiUrl = 'https://localhost:7181/api/Sales';
  private apiUrlApi = 'https://localhost:7181/api';

  constructor(private http: HttpClient) {}

  getAllSales(): Observable<Sale[]> {
    return this.http.get<any>(this.apiUrl).pipe(
      map(response => {
        console.log('API Response:', response); // Debug
        if (response && response.data && response.data.data && response.data.data.sales) {
          return response.data.data.sales;
        }
        return [];
      }),
      catchError(error => {
        console.error('Service Error:', error);
        throw error; // Propaga o erro para o componente tratar
      })
    );
  }

  getSaleById(id: number): Observable<Sale> {
    return this.http.get<any>(`${this.apiUrl}/${id}`).pipe(
      map(response => response.data.data)
    );
  }

  createSale(sale: Sale): Observable<Sale> {
    const payload = {
      id: 0,
      saleNumber: sale.saleNumber,
      saleDate: sale.saleDate,
      customer: sale.customer,
      totalAmount: sale.totalAmount,
      branch: sale.branch,
      items: sale.items.map(item => ({
        productId: item.productId,
        quantity: item.quantity,
        unitPrice: item.unitPrice,
        discount: item.discount || 0,
        total: item.total
      }))
    };

    return this.http.post<any>(this.apiUrl, payload).pipe(
      map(response => response.data),
      catchError(error => {
        console.error('Error in createSale:', error);
        throw error;
      })
    );
  }

  updateSale(id: number, sale: Sale): Observable<Sale> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, sale).pipe(
      map(response => response.data.data)
    );
  }

  deleteSale(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  deleteSaleItem(itemId: number): Observable<void> {
    return this.http.delete<any>(`${this.apiUrlApi}/SaleItem/${itemId}`).pipe(
      map(response => {
        console.log('Delete Response:', response);
        if (!response.success) {
          throw new Error(response.message);
        }
      }),
      catchError(error => {
        console.error('Error deleting sale item:', error);
        throw error;
      })
    );
  }

  searchSales(saleNumber?: string, startDate?: string, endDate?: string): Observable<Sale[]> {
    let params = new HttpParams();
    if (saleNumber) params = params.set('saleNumber', saleNumber);
    if (startDate) params = params.set('startDate', startDate);
    if (endDate) params = params.set('endDate', endDate);

    return this.http.get<Sale[]>(`${this.apiUrl}/search`, { params }).pipe(
      map(response => {
        console.log('Search Response:', response);
        return response || [];
      }),
      catchError(error => {
        console.error('Error searching sales:', error);
        return [];
      })
    );
  }

  getSaleItems(saleId: number) {
    interface ItemsResponse {
      data: {
        data: Array<{
          id: number;
          saleId: number;
          productId: number;
          product: {
            id: number;
            name: string;
          };
          quantity: number;
          unitPrice: number;
          total: number;
        }>;
        success: boolean;
        message: string;
        errors: any[];
      };
      success: boolean;
      message: string;
      errors: any[];
    }

    return this.http.get<ItemsResponse>(`${this.apiUrl}/${saleId}/items`).pipe(
      map(response => {
        console.log('Items Response:', response);
        return response?.data?.data || [];
      }),
      catchError(error => {
        console.error('Error fetching sale items:', error);
        throw error;
      })
    );
  }

  addSaleItem(item: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrlApi}/SaleItem`, item).pipe(
      map(response => {
        console.log('Add Item Response:', response);
        return response;
      }),
      catchError(error => {
        console.error('Error adding sale item:', error);
        throw error;
      })
    );
  }

  getProducts(): Observable<any[]> {
    return this.http.get<any>(`${this.apiUrlApi}/Product`).pipe(
      map(response => {
        console.log('Products Response:', response);
        return response?.data?.data || [];
      }),
      catchError(error => {
        console.error('Error fetching products:', error);
        throw error;
      })
    );
  }
}
