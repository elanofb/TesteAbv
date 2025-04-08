using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSales
{
    public class GetSalesHandler : IRequestHandler<GetSalesCommand, GetSalesResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public GetSalesHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<GetSalesResult> Handle(GetSalesCommand request, CancellationToken cancellationToken)
        {
            var totalCount = await _saleRepository.CountAsync();
            var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

            var sales = await _saleRepository.GetPagedAsync(request.Page, request.PageSize);
            var salesResult = _mapper.Map<List<GetSaleResult>>(sales);

            return new GetSalesResult
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                Page = request.Page,
                PageSize = request.PageSize,
                Sales = salesResult
            };
        }
    }
}
