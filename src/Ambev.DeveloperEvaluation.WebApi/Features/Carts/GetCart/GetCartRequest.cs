using MediatR;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.GetCart
{
    public class GetCartRequest : IRequest<GetCartResponse>
    {
        public int Id { get; set; }
    }
}
