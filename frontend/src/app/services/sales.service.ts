import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map, catchError } from 'rxjs';
import { Sale } from '../models/sale.model';

@Injectable({
  providedIn: 'root'
})
export class SalesService {
  private apiUrl = 'https://localhost:7181/api/sales';

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
    // Remove os IDs do payload para deixar o backend gerar
    const payload = {
      saleNumber: sale.saleNumber,
      saleDate: sale.saleDate,
      customer: sale.customer,
      totalAmount: sale.totalAmount,
      branch: sale.branch,
      items: sale.items.map(item => ({
        productId: item.productId,
        quantity: item.quantity,
        unitPrice: item.unitPrice,
        discount: item.discount,
        total: item.total
      }))
    };

    return this.http.post<any>(this.apiUrl, payload).pipe(
      map(response => response.data)
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
}
