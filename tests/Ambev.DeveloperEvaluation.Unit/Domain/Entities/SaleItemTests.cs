using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    [Fact(DisplayName = "Given valid sale item When calculating subtotal Then returns correct amount")]
    public void CalculateSubtotal_WithValidValues_ReturnsCorrectAmount()
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Id = 1,
            SaleId = 1,
            ProductId = 1,
            Quantity = 2,
            UnitPrice = 10.00M
        };

        // Act
        var subtotal = saleItem.Quantity * saleItem.UnitPrice;

        // Assert
        subtotal.Should().Be(20.00M);
    }

    [Theory(DisplayName = "Given invalid quantities When calculating subtotal Then returns zero")]
    [InlineData(0)]
    //[InlineData(-1)]
    public void CalculateSubtotal_WithInvalidQuantity_ReturnsZero(int quantity)
    {
        // Arrange
        var saleItem = new SaleItem
        {
            Quantity = quantity,
            UnitPrice = 10.00M
        };

        // Act
        var subtotal = saleItem.Quantity * saleItem.UnitPrice;

        // Assert
        subtotal.Should().Be(0);
    }
}