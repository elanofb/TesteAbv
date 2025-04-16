using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Common.Security;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.CreateSaleItem;

/// <summary>
/// Handler for processing CreateSaleItemCommand requests
/// </summary>
public class CreateSaleItemHandler : IRequestHandler<CreateSaleItemCommand, CreateSaleItemResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleItemRepository _saleitemRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    /// <summary>
    /// Initializes a new instance of CreateSaleItemHandler
    /// </summary>
    /// <param name="saleitemRepository">The saleitem repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for CreateSaleItemCommand</param>
    public CreateSaleItemHandler(ISaleItemRepository saleitemRepository, ISaleRepository saleRepository, IMapper mapper)
    {
        _saleitemRepository = saleitemRepository;
        _saleRepository = saleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the CreateSaleItemCommand request
    /// </summary>
    /// <param name="command">The CreateSaleItem command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created saleitem details</returns>
    public async Task<CreateSaleItemResult> Handle(CreateSaleItemCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleItemCommandValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var existingSale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
        if (existingSale == null)
            throw new InvalidOperationException($"Sale with ID {command.SaleId} doesn't exists");

        //var existingSaleItem = await _saleitemRepository.GetByIdAsync(command.SaleId, cancellationToken);
        //if (existingSaleItem != null)
        //    throw new InvalidOperationException($"SaleItem with ID {command.SaleId} already exists");

        var saleitem = _mapper.Map<SaleItem>(command);
        //saleitem.Password = _passwordHasher.HashPassword(command.Password);

        var createdSaleItem = await _saleitemRepository.CreateAsync(saleitem, cancellationToken);
        var result = _mapper.Map<CreateSaleItemResult>(createdSaleItem);
        return result;
    }
}
