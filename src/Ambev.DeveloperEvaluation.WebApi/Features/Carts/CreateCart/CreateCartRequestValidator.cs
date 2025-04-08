using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.CreateCart
{
    public class CreateCartRequestValidator : AbstractValidator<CreateCartRequest>
    {
        public CreateCartRequestValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();//.GreaterThan(0);
            RuleFor(x => x.Date).NotEmpty();
            RuleForEach(x => x.Products).ChildRules(p =>
            {
                p.RuleFor(p => p.ProductId).GreaterThan(0);
                p.RuleFor(p => p.Quantity).GreaterThan(0);
            });
        }
    }
}
