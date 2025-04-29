using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using System.Reflection.Metadata;
using Xunit;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class ProductTests
{
    [Fact(DisplayName = "Given valid product When creating Then has correct properties")]
    public void CreateProduct_WithValidData_HasCorrectProperties()
    {
        // Arrange
        var product = new Product
        {
            Id = 1,
            Name = "Test Product",
            Description = "Test Description",
            UnitPrice = 10.00M,
        };

        // Assert
        product.Should().NotBeNull();
        product.Id.Should().Be(1);
        product.Name.Should().Be("Test Product");
        product.Description.Should().Be("Test Description");
        product.UnitPrice.Should().Be(10.00M);
    }

    [Theory(DisplayName = "Given invalid price When validating Then throws validation exception")]
    [InlineData(0)]
    [InlineData(-1)]
    public void Validate_WithInvalidPrice_ThrowsValidationException(decimal price)
    {
        // Arrange
        var product = new Product { UnitPrice = price };
        var validator = new ProductValidator();

        // Act
        var result = validator.Validate(product);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "UnitPrice");
    }
}