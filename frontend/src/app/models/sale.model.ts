export interface SaleItem {
  id?: number;
  productId: number;
  quantity: number;
  unitPrice: number;
  discount: number;
  total: number;
  saleId?: number;
}

export interface Sale {
  id?: number;
  saleNumber: string;
  saleDate: string;
  customer: string;
  totalAmount: number;
  branch: string;
  items: SaleItem[];
}