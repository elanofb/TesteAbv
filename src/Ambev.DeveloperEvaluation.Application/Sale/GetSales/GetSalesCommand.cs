using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesCommand : IRequest<GetSalesResult>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
