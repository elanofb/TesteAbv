using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItemsBySaleId;

public class GetSaleItemsBySaleIdCommand : IRequest<List<GetSaleItemsBySaleIdResult>>
{
    public int SaleId { get; set; }
}
