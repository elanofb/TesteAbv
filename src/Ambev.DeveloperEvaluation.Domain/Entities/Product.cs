namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public decimal UnitPrice;
        public ICollection<SaleItem> SaleItems { get; set; }
    }
}
