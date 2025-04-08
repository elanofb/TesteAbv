namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    public class CreateCartResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CreateCartProductResponse> Products { get; set; } = new();
    }

    public class CreateCartProductResponse
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
