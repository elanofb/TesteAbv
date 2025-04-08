using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts
{
    public class GetCartsResponse
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<GetCartResponse> Data { get; set; } = new();
    }
}
