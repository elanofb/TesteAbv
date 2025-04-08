﻿using AutoMapper;
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

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly IMessageBusService _messageBusService;
    private readonly SaleDiscountService _discountService;

    public CreateSaleHandler(ISaleRepository saleRepository,
                                IMapper mapper,
                                ISaleItemRepository saleItemRepository,
                                IMessageBusService messageBusService,
                                SaleDiscountService discountService)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _saleItemRepository = saleItemRepository;
        _messageBusService = messageBusService;
        _discountService = discountService;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
        if (existingSale != null)
            throw new InvalidOperationException($"Sale with ID {command.Id} already exists");

        // Busca o último ID de venda 
        var sales = await _saleRepository.GetAllAsync(cancellationToken);
        var lastSale = sales
            .OrderByDescending(s => s.Id)
            .FirstOrDefault();
        var nextSaleId = (lastSale?.Id ?? 0) + 1;
        var nextItemId = 1;                  

        var sale = _mapper.Map<Sale>(command);
        sale.Id = nextSaleId;

        // Aplicar desconto antes de salvar a venda
        _discountService.ApplyDiscounts(sale.Items);

        var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);

        // Publicando evento no Rebus após salvar a venda.
        await _messageBusService.PublishEvent(new OrderCreatedEvent(createdSale.SaleNumber, createdSale.Customer, createdSale.TotalAmount));

        var result = _mapper.Map<CreateSaleResult>(createdSale);

        return result;
    }
}
