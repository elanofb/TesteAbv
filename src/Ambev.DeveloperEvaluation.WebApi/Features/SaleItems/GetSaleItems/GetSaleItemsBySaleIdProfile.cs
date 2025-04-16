using AutoMapper;
using Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItemsBySaleId;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.GetSaleItemsBySaleId;

public class GetSaleItemsBySaleIdProfile : Profile
{
    public GetSaleItemsBySaleIdProfile()
    {
        CreateMap<GetSaleItemsBySaleIdRequest, GetSaleItemsBySaleIdCommand>();
    }
}
