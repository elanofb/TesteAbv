using AutoMapper;
using Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Application.Carts.CreateCart;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    public class CreateCartProfile : Profile
    {
        public CreateCartProfile()
        {
            CreateMap<CreateCartRequest, CreateCartCommand>();
            CreateMap<CreateCartProductRequest, CartProduct>();
            CreateMap<CreateCartResult, CreateCartResponse>();
            CreateMap<CartProduct, CreateCartProductResponse>();
            CreateMap<CreateCartCommand, Cart>();
            CreateMap<Cart, CreateCartResult>();
        }
    }
}
