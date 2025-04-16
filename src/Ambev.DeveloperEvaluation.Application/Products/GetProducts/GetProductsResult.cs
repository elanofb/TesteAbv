using Ambev.DeveloperEvaluation.Application.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts
{
    public class GetProductsResult
    {
        public List<GetProductResult> Products { get; set; } = new();
    }
}
