using Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart;
using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCarts
{
    public class GetCartsRequest : IRequest<GetCartsResponse>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Order { get; set; } = "id asc";
    }
}
