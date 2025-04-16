export interface Product {
  id?: number;
  name: string;
  description: string;
  price: number;  // Changed from unitPrice to price
  isAvailable: boolean;
}