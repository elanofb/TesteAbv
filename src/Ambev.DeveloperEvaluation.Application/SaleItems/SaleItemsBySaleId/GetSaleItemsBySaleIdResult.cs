namespace Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItemsBySaleId;

public class GetSaleItemsBySaleIdResult
{
    public int Id { get; set; }
    public int SaleId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string ProductName { get; set; }
}
