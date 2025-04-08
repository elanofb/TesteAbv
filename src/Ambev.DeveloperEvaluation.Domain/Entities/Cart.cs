public class Cart
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime Date { get; set; }
    public List<CartProduct> Products { get; set; } = new();
}

public class CartProduct
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
