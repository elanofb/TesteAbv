namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart {
    public class UpdateCartResult
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CartProduct> Products { get; set; } = new();
    }
}