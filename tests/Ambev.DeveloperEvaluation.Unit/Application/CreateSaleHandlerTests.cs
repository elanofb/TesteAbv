using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Services;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly ISaleItemRepository _saleItemRepository;
    private readonly IMapper _mapper;
    private readonly IMessageBusService _messageBusService;
    private readonly SaleDiscountService _discountService;
    private readonly ILogger<CreateSaleHandler> _logger;
    private readonly CreateSaleHandler _handler;
    private int _lastSaleId;
    private int _lastSaleItemId;
    private int _lastProductId;

    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _saleItemRepository = Substitute.For<ISaleItemRepository>();
        _mapper = Substitute.For<IMapper>();
        _messageBusService = Substitute.For<IMessageBusService>();
        _discountService = Substitute.For<SaleDiscountService>();
        _logger = Substitute.For<ILogger<CreateSaleHandler>>();

        // Configuração inicial dos IDs
        ConfigureInitialIds();
        
        _handler = new CreateSaleHandler(
            _saleRepository, 
            _mapper, 
            _saleItemRepository,
            _messageBusService,
            _discountService,
            _logger);
    }

    private void ConfigureInitialIds()
    {
        // Simula a busca dos últimos IDs
        _lastSaleId = 8; // Último ID real da tabela Sale
        _lastSaleItemId = _lastSaleId * 2; // Exemplo: cada venda tem em média 2 itens
        _lastProductId = _lastSaleId * 3; // Exemplo: mantém uma proporção com produtos

        // Configura os mocks para retornar os IDs
        _saleRepository.GetLastIdAsync(Arg.Any<CancellationToken>()).Returns(_lastSaleId);
        _saleItemRepository.GetLastIdAsync(Arg.Any<CancellationToken>()).Returns(_lastSaleItemId);
    }

    private int GetNextId(ref int lastId) => ++lastId;

    private async Task<int> GetNextSaleIdAsync() 
    {
        var lastId = await _saleRepository.GetLastIdAsync(CancellationToken.None);
        return GetNextId(ref _lastSaleId);
    }

    private async Task<int> GetNextSaleItemIdAsync() 
    {
        var lastId = await _saleItemRepository.GetLastIdAsync(CancellationToken.None);
        return GetNextId(ref _lastSaleItemId);
    }

    private int GetNextProductId() => GetNextId(ref _lastProductId);

    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var nextSaleId = await GetNextSaleIdAsync();
        var nextSaleItemId = await GetNextSaleItemIdAsync();
        var nextProductId = GetNextProductId();

        var command = new CreateSaleCommand
        {
            Customer = "Test Customer",
            SaleNumber = "SALE001",
            SaleDate = DateTime.Now,
            Branch = "Main Branch",
            Items = new List<SaleItem>
            {
                new()
                {
                    Id = nextSaleItemId,
                    SaleId = nextSaleId,
                    ProductId = nextProductId,
                    Quantity = 2,
                    UnitPrice = 10.00M
                }
            },
            TotalAmount = 20.00M
        };

        var sale = new Sale
        {
            Id = nextSaleId,
            Customer = command.Customer,
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            Branch = command.Branch,
            Items = command.Items,
            TotalAmount = command.TotalAmount
        };


        var result = new CreateSaleResult { Id = sale.Id };

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createSaleResult.Should().NotBeNull();
        await _saleRepository.Received(1).CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateSaleCommand(); // Empty command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact(DisplayName = "Given sale with items When creating Then calculates total correctly")]
    public async Task Handle_SaleWithItems_CalculatesTotalCorrectly()
    {
        // Given
        var nextSaleId = await GetNextSaleIdAsync();
        var nextSaleItemId = await GetNextSaleItemIdAsync();
        var firstProductId = GetNextProductId();
        var secondProductId = GetNextProductId();

        var command = new CreateSaleCommand
        {
            // Removido Id do command
            Customer = "Test Customer",
            SaleNumber = "SALE001",
            SaleDate = DateTime.Now,
            Branch = "Main Branch",
            Items = new List<SaleItem>
            {
                new() 
                { 
                    Id = nextSaleItemId,
                    SaleId = nextSaleId,
                    ProductId = firstProductId,
                    Quantity = 2, 
                    UnitPrice = 10.00M 
                },
                new() 
                { 
                    Id = nextSaleItemId + 1,
                    SaleId = nextSaleId,
                    ProductId = secondProductId,
                    Quantity = 1, 
                    UnitPrice = 20.00M 
                }
            },
            TotalAmount = 40.00M
        };

        var sale = new Sale
        {
            Id = nextSaleId,
            Customer = command.Customer,
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            Branch = command.Branch,
            Items = command.Items,
            TotalAmount = command.TotalAmount
        };

        // Configurar o mapper para retornar a venda corretamente
        _mapper.Map<Sale>(Arg.Any<CreateSaleCommand>()).Returns(sale);
        _saleRepository.CreateAsync(Arg.Any<Sale>(), Arg.Any<CancellationToken>())
            .Returns(sale);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        await _saleRepository.Received(1).CreateAsync(
            Arg.Is<Sale>(s => s.TotalAmount == 40.00M),
            Arg.Any<CancellationToken>());
    }
}