using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.DeleteCart;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.DeleteCart
{
    public class DeleteCartProfile : Profile
    {
        public DeleteCartProfile()
        {
            CreateMap<int, DeleteCartCommand>()
                .ConstructUsing(id => new DeleteCartCommand(id));
        }
    }
}
