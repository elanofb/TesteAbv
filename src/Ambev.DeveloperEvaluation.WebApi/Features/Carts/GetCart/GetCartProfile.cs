using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart
{
    public class GetCartProfile : Profile
    {
        public GetCartProfile()
        {
            CreateMap<GetCartResult, GetCartResponse>();
            CreateMap<CartProduct, GetCartProductResponse>();
            CreateMap<Cart, GetCartResult>();
            CreateMap<GetCartCommand, GetCartRequest>();
            CreateMap<Cart, GetCartResponse>();
        }
    }
}
