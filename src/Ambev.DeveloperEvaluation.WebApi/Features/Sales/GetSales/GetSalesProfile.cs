using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.GetSales;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSales
{
    public class GetSalesProfile : Profile
    {
        public GetSalesProfile()
        {
            //CreateMap<GetSalesCommand, GetSalesRequest>();
            CreateMap<GetSalesRequest, GetSalesCommand>();
            CreateMap<GetSalesResult, GetSalesResponse>();
        }
    }
}
