namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart {
    public class CreateCartResult
    {
        public int Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CartProduct> Products { get; set; } = new();
    }
}