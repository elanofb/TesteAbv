// ... existing code ...
export class SalesService {
  // ... existing methods ...

  getSaleItems(saleId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/sales/${saleId}/items`);
  }
}