using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Products.GetProduct;

namespace Ambev.DeveloperEvaluation.Application.Products.GetProducts
{
    public class GetProductsHandler : IRequestHandler<GetProductsCommand, GetProductsResult>
    {
        private readonly IProductRepository _ProductRepository;
        private readonly IMapper _mapper;

        public GetProductsHandler(IProductRepository ProductRepository, IMapper mapper)
        {
            _ProductRepository = ProductRepository;
            _mapper = mapper;
        }

        public async Task<GetProductsResult> Handle(GetProductsCommand request, CancellationToken cancellationToken)
        {
            var Products = await _ProductRepository.GetAllAsync();
            var ProductsResult = _mapper.Map<List<GetProductResult>>(Products);

            return new GetProductsResult
            {
                Products = ProductsResult
            };
        }
    }
}
