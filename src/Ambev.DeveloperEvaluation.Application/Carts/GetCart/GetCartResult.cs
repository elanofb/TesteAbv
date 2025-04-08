namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart {
    public class GetCartResult
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CartProduct> Products { get; set; } = new();
    }
}