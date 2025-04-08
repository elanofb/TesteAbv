using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Carts.CreateCart {
    public class CreateCartValidator : AbstractValidator<CreateCartCommand>
    {
        public CreateCartValidator()
        {
            RuleFor(x => x.UserId).NotEmpty(); //GreaterThan(0);
            RuleFor(x => x.Date).NotEmpty();
            RuleForEach(x => x.Products).ChildRules(products =>
            {
                products.RuleFor(p => p.ProductId).GreaterThan(0);
                products.RuleFor(p => p.Quantity).GreaterThan(0);
            });
        }
    }
}
