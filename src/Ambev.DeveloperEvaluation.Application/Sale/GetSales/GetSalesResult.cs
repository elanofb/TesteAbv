using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesResult
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<GetSaleResult> Sales { get; set; } = new();
    }
}
