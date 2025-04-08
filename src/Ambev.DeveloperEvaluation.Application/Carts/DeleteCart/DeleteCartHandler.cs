using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Carts.DeleteCart {
    public class DeleteCartHandler : IRequestHandler<DeleteCartCommand, Unit>
    {
        private readonly ICartRepository _repo;

        public DeleteCartHandler(ICartRepository repo)
        {
            _repo = repo;
        }

        public async Task<Unit> Handle(DeleteCartCommand request, CancellationToken cancellationToken)
        {
            await _repo.DeleteAsync(request.Id);
            return Unit.Value;
        }
    }
}