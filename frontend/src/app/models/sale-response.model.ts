export interface SaleResponse {
  data: {
    totalCount: number;
    totalPages: number;
    page: number;
    pageSize: number;
    sales: Array<{
      id: number;
      saleNumber: string;
      saleDate: string;
      customer: string;
      totalAmount: number;
      branch: string;
      items: any[];
    }>;
  };
  success: boolean;
  message: string;
  errors: any[];
}