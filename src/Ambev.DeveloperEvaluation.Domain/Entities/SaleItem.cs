namespace Ambev.DeveloperEvaluation.Domain.Entities
{  
    public class SaleItem
    {
        public int Id { get; set; } // EF Core irá configurar como Identity
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public Product Product { get; set; }
        public Sale Sale { get; set; }
    }
}