// using MediatR;
// using Ambev.DeveloperEvaluation.Domain.Entities;
// using Ambev.DeveloperEvaluation.Domain.Interfaces;
// using Ambev.DeveloperEvaluation.Domain.Results;

// namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
// {
//     public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, Result<Sale>>
//     {
//         private readonly IRepository<Sale> _saleRepository;

//         public CreateSaleHandler(IRepository<Sale> saleRepository)
//         {
//             _saleRepository = saleRepository;
//         }

//         public async Task<Result<Sale>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
//         {
//             var sale = new Sale
//             {
//                 SaleNumber = request.SaleNumber,
//                 SaleDate = request.SaleDate,
//                 Customer = request.Customer,
//                 TotalAmount = request.TotalAmount,
//                 Branch = request.Branch,
//                 Items = request.Items.Select(item => new SaleItem
//                 {
//                     ProductId = item.ProductId,
//                     Quantity = item.Quantity,
//                     UnitPrice = item.UnitPrice,
//                     Discount = item.Discount,
//                     Total = item.Total
//                 }).ToList()
//             };

//             await _saleRepository.AddAsync(sale, cancellationToken);
//             await _saleRepository.SaveChangesAsync(cancellationToken);

//             return Result.Success(sale);
//         }
//     }
// }