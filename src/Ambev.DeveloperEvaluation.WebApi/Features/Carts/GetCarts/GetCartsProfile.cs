using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;
using Ambev.DeveloperEvaluation.Application.Carts.GetCarts;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts
{
    public class GetCartsProfile : Profile
    {
        public GetCartsProfile()
        {
            CreateMap<GetCartsResult, GetCartsResponse>();
            CreateMap<GetCartResult, GetCartResponse>();
            CreateMap<GetCartsRequest, GetCartsCommand>();
            CreateMap<GetCartsResult, GetCartsResponse>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.Data));
        }
    }
}
