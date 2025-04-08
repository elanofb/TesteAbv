using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart {
    public class CreateCartCommand : IRequest<CreateCartResult>
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
        public List<CartProduct> Products { get; set; } = new();
    }
}