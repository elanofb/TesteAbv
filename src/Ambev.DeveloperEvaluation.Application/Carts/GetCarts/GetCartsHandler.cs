using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Carts.GetCart;

namespace Ambev.DeveloperEvaluation.Application.Carts.GetCarts {
    public class GetCartsHandler : IRequestHandler<GetCartsCommand, GetCartsResult>
    {
        private readonly ICartRepository _repo;
        private readonly IMapper _mapper;

        public GetCartsHandler(ICartRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<GetCartsResult> Handle(GetCartsCommand request, CancellationToken cancellationToken)
        {
            var total = await _repo.CountAsync();
            var pages = (int)Math.Ceiling(total / (double)request.PageSize);
            var carts = await _repo.GetPagedAsync(request.Page, request.PageSize, request.Order);
            return new GetCartsResult
            {
                TotalItems = total,
                CurrentPage = request.Page,
                TotalPages = pages,
                Data = _mapper.Map<List<GetCartResult>>(carts)
            };
        }
        //public async Task<GetCartsResponse> Handle(GetCartsRequest request, CancellationToken cancellationToken)
        //{
        //    var result = await _repo.GetPagedAsync(request.Page, request.PageSize, request.Order, cancellationToken);

        //    return new GetCartsResponse
        //    {
        //        TotalItems = result.TotalItems,
        //        CurrentPage = result.CurrentPage,
        //        TotalPages = result.TotalPages,
        //        Data = _mapper.Map<List<GetCartResponse>>(result.Items)
        //    };
        //}
    }
}