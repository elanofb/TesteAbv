using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Validation;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
//using Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

namespace Ambev.DeveloperEvaluation.Unit.Application;

public class CreateProductHandlerTests
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly CreateProductHandler _handler;
    private readonly IValidator<CreateProductCommand> _validator;

    public CreateProductHandlerTests()
    {
        _productRepository = Substitute.For<IProductRepository>();
        _mapper = Substitute.For<IMapper>();        
        _validator = new CreateProductCommandValidator();
        _handler = new CreateProductHandler(_productRepository, _mapper,_validator);
    }

    [Fact(DisplayName = "Given valid product data When creating Then returns success")]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        // Arrange
        var command = new CreateProductCommand
        {
            Name = "Test Product",
            Description = "Test Description",
            UnitPrice = 10.00M,
            IsAvailable = true
        };

        var product = new Product
        {
            Id = 1,
            Name = command.Name,
            Description = command.Description,
            UnitPrice = command.UnitPrice,
            IsAvailable = command.IsAvailable
        };

        var result = new CreateProductResult { Id = product.Id };

        _mapper.Map<Product>(command).Returns(product);
        _mapper.Map<CreateProductResult>(product).Returns(result);
        _productRepository.CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>())
            .Returns(product);

        // Act
        var createResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        createResult.Should().NotBeNull();
        createResult.Id.Should().Be(product.Id);
        await _productRepository.Received(1).CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    [Fact(DisplayName = "Given invalid product data When creating Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        var command = new CreateProductCommand 
        { 
            Name = "", // invalid name
            UnitPrice = -1 // invalid price
        };
        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();       
    }
}