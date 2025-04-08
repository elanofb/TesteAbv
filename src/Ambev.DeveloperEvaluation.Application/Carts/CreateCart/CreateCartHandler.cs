using MediatR;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart {
    public class CreateCartHandler : IRequestHandler<CreateCartCommand, CreateCartResult>
    {
        private readonly ICartRepository _repo;
        private readonly IMapper _mapper;

        public CreateCartHandler(ICartRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CreateCartResult> Handle(CreateCartCommand request, CancellationToken ct)
        {
            var cart = _mapper.Map<Cart>(request);
            await _repo.AddAsync(cart);
            return _mapper.Map<CreateCartResult>(cart);
        }
    }
}