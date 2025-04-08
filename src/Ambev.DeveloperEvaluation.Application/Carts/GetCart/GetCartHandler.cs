using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCart {
    public class GetCartHandler : IRequestHandler<GetCartCommand, GetCartResult>
    {
        private readonly ICartRepository _repo;
        private readonly IMapper _mapper;

        public GetCartHandler(ICartRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<GetCartResult> Handle(GetCartCommand request, CancellationToken cancellationToken)
        {
            var cart = await _repo.GetByIdAsync(request.Id);
            if (cart == null)
                throw new KeyNotFoundException("Cart not found");

            return _mapper.Map<GetCartResult>(cart);
        }
    }
}