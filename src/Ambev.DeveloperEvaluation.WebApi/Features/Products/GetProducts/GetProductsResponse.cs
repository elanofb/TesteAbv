using Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts
{
    public class GetProductsResponse
    {
        public List<GetProductResponse> Products { get; set; } = new();
    }
}
