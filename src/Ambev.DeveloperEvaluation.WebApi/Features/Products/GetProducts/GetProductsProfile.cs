using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Products.GetProducts;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Products.GetProducts
{
    public class GetProductsProfile : Profile
    {
        public GetProductsProfile()
        {
            //CreateMap<GetProductsCommand, GetProductsRequest>();
            CreateMap<GetProductsRequest, GetProductsCommand>();
            CreateMap<GetProductsResult, GetProductsResponse>();
        }
    }
}
