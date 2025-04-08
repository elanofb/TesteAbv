using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart {
    public class DeleteCartCommand : IRequest<Unit>
    {
        public int Id { get; set; }

        public DeleteCartCommand(int id)
        {
            Id = id;
        }
    }
}