using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.UpdateCart {
    public class UpdateCartValidator : AbstractValidator<UpdateCartCommand>
    {
        public UpdateCartValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Date).NotEmpty();
            RuleForEach(x => x.Products).ChildRules(products =>
            {
                products.RuleFor(p => p.ProductId).GreaterThan(0);
                products.RuleFor(p => p.Quantity).GreaterThan(0);
            });
        }
    }
}
