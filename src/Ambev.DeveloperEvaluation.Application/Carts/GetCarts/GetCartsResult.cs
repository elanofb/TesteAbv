using Ambev.DeveloperEvaluation.Application.Carts.GetCart;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts {
    public class GetCartsResult
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<GetCartResult> Data { get; set; } = new();
    }
}
