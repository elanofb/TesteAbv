import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable, map, catchError } from 'rxjs';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  private apiUrl = 'https://localhost:7181/api/Products';
  private httpOptions = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json'
    })
  };

  constructor(private http: HttpClient) {}

  createProduct(product: any): Observable<any> {
    return this.http.post<any>(this.apiUrl, product, this.httpOptions);
  }

  getAllProducts(): Observable<any[]> {
    console.log('Calling getAllProducts');
    return this.http.get<any>(this.apiUrl).pipe(
      map(response => {
        console.log('Raw API Response:', response);
        if (response?.data?.data?.products) {
          const products = response.data.data.products;
          console.log('Parsed products:', products);
          return products;
        }
        console.log('No products found in response');
        return [];
      }),
      catchError(error => {
        console.error('Error in getAllProducts:', error);
        throw error;
      })
    );
  }

  getProductById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`).pipe(
      map(response => response?.data?.data || null)
    );
  }

  // createProduct(product: Product): Observable<Product> {
  //   return this.http.post<any>(this.apiUrl, product).pipe(
  //     map(response => response.data)
  //   );
  // }

  updateProduct(id: number, product: Product): Observable<Product> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, product).pipe(
      map(response => response.data)
    );
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  searchProducts(searchTerm: string): Observable<Product[]> {
    let params = new HttpParams();
    if (searchTerm) params = params.set('searchTerm', searchTerm);

    return this.http.get<Product[]>(`${this.apiUrl}/search`, { params }).pipe(
      map(response => response || [])
    );
  }
}