using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts
{
    public class GetProductsCommand : IRequest<GetProductsResult>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
