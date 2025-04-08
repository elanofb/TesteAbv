using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart {
    public class GetCartCommand : IRequest<GetCartResult>
    {
        public int Id { get; set; }

        public GetCartCommand(int id)
        {
            Id = id;
        }
    }
}