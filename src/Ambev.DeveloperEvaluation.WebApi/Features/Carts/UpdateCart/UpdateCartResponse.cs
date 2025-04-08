namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    public class UpdateCartResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public List<UpdateCartProductResponse> Products { get; set; } = new();
    }

    public class UpdateCartProductResponse
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
