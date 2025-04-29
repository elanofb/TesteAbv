using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Validation;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Products.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
                   .NotEmpty()
                   .WithMessage("Name is required");

        RuleFor(x => x.UnitPrice)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero");
    }
}