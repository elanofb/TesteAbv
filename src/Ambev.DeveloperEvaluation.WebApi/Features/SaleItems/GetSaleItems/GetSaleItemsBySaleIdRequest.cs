using MediatR;
using Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItemsBySaleId;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.GetSaleItemsBySaleId;

public class GetSaleItemsBySaleIdRequest : IRequest<List<GetSaleItemsBySaleIdResult>>
{
    public int SaleId { get; set; }
}
