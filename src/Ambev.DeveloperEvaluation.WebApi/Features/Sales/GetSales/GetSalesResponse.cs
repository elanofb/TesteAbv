using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales
{
    public class GetSalesResponse
    {
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public List<GetSaleResponse> Sales { get; set; } = new();
    }
}
