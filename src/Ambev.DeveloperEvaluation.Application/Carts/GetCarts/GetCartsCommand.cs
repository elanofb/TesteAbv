using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts {
    public class GetCartsCommand : IRequest<GetCartsResult>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string Order { get; set; } = "id asc";
    }
}