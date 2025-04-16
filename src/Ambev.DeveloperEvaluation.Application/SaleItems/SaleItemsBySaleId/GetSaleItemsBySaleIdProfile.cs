using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItemsBySaleId;

public class GetSaleItemsBySaleIdProfile : Profile
{
    public GetSaleItemsBySaleIdProfile()
    {
        //CreateMap<Domain.Entities.SaleItem, GetSaleItemsBySaleIdResult>();
        CreateMap<Domain.Entities.SaleItem, GetSaleItemsBySaleIdResult>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name));

    }
}
