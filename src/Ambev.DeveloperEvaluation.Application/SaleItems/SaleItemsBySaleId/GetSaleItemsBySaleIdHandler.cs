using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
namespace Ambev.DeveloperEvaluation.Application.SaleItems.GetSaleItemsBySaleId;

public class GetSaleItemsBySaleIdHandler : IRequestHandler<GetSaleItemsBySaleIdCommand, List<GetSaleItemsBySaleIdResult>>
{
    private readonly ISaleItemRepository _repo;
    private readonly IMapper _mapper;

    public GetSaleItemsBySaleIdHandler(ISaleItemRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<List<GetSaleItemsBySaleIdResult>> Handle(GetSaleItemsBySaleIdCommand request, CancellationToken cancellationToken)
    {
        var saleItems = await _repo.GetBySaleIdAsync(request.SaleId);
        //var saleItems2 = await _repo.GetBySaleIdWithProductAsync(request.SaleId);
        return _mapper.Map<List<GetSaleItemsBySaleIdResult>>(saleItems);
    }
}
