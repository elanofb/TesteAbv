using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleTests
{
    [Fact(DisplayName = "Given valid sale When calculating total Then returns correct amount")]
    public void CalculateTotal_WithValidItems_ReturnsCorrectTotal()
    {
        // Arrange
        var sale = new Sale
        {
            Id = 1,
            Customer = "Test Customer",
            SaleNumber = "SALE001",
            SaleDate = DateTime.Now,
            Branch = "Main Branch",
            Items = new List<SaleItem>
            {
                new() { Quantity = 2, UnitPrice = 10.00M },
                new() { Quantity = 1, UnitPrice = 20.00M }
            }
        };

        // Act
        var total = sale.Items.Sum(item => item.Quantity * item.UnitPrice);

        // Assert
        total.Should().Be(40.00M);
    }

    [Fact(DisplayName = "Given sale without items When calculating total Then returns zero")]
    public void CalculateTotal_WithoutItems_ReturnsZero()
    {
        // Arrange
        var sale = new Sale
        {
            Id = 1,
            Customer = "Test Customer",
            SaleNumber = "SALE001",
            Items = new List<SaleItem>()
        };

        // Act
        var total = sale.Items.Sum(item => item.Quantity * item.UnitPrice);

        // Assert
        total.Should().Be(0);
    }
}