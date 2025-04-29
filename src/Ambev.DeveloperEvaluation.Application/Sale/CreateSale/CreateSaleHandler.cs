using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Domain.Events;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly IMessageBusService _messageBusService;
    private readonly SaleDiscountService _discountService;
    private readonly ILogger<CreateSaleHandler> _logger;

    public CreateSaleHandler(ISaleRepository saleRepository,
                                IMapper mapper,
                                ISaleItemRepository saleItemRepository,
                                IMessageBusService messageBusService,
                                SaleDiscountService discountService,
                                ILogger<CreateSaleHandler> logger)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _saleItemRepository = saleItemRepository;
        _messageBusService = messageBusService;
        _discountService = discountService;
        _logger = logger;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting sale creation for command {@Command}", command);

            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid){
                _logger.LogWarning("Validation failed for sale command: {@ValidationErrors}", validationResult.Errors);
                throw new ValidationException(validationResult.Errors);
            }

            var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (existingSale != null){
                _logger.LogWarning("Attempted to create duplicate sale with ID {SaleId}", command.Id);
                throw new InvalidOperationException($"Sale with ID {command.Id} already exists");
            }

            // Busca o último ID de venda 
            var lastSale = await _saleRepository.GetLastIdAsync(cancellationToken);            
            var nextSaleId = lastSale + 1;

            var sale = _mapper.Map<Sale>(command);
            sale.Id = nextSaleId;

            _logger.LogInformation("Applying discounts for sale {SaleId}", sale.Id);
            // Aplicar desconto antes de salvar a venda
            _discountService.ApplyDiscounts(sale.Items);

            sale.SaleDate = DateTime.SpecifyKind(sale.SaleDate, DateTimeKind.Utc);

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
            _logger.LogInformation("Sale created successfully with ID {SaleId}", createdSale.Id);

            // Publicando evento no Rebus após salvar a venda.
            await _messageBusService.PublishEvent(new OrderCreatedEvent(createdSale.SaleNumber, createdSale.Customer, createdSale.TotalAmount));
            _logger.LogInformation("Order created event published for sale {SaleId}", createdSale.Id);

            var result = _mapper.Map<CreateSaleResult>(createdSale);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating sale for command {@Command}", command);
            throw;
        }
    }
}
