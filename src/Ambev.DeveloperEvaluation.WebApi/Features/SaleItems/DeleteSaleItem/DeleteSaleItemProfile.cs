using Ambev.DeveloperEvaluation.Application.SaleItems.DeleteSaleItem;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.SaleItems.DeleteSaleItem;

public class DeleteSaleItemProfile : Profile
{
    public DeleteSaleItemProfile()
    {
        CreateMap<DeleteSaleItemRequest, DeleteSaleItemCommand>();
    }
}
