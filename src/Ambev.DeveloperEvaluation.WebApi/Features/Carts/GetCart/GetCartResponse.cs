namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart
{
    public class GetCartResponse
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<GetCartProductResponse> Products { get; set; } = new();
    }

    public class GetCartProductResponse
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
