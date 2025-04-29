using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero");

        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Name is required");
    }
}