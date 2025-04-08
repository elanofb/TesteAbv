using MediatR;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart {
    public class UpdateCartHandler : IRequestHandler<UpdateCartCommand, UpdateCartResult>
    {
        private readonly ICartRepository _repo;
        private readonly IMapper _mapper;

        public UpdateCartHandler(ICartRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UpdateCartResult> Handle(UpdateCartCommand request, CancellationToken cancellationToken)
        {
            var cart = _mapper.Map<Cart>(request);
            await _repo.UpdateAsync(cart);
            return _mapper.Map<UpdateCartResult>(cart);
        }
    }
}
