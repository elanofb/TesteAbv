using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Carts.UpdateCart
{
    public class UpdateCartRequestValidator : AbstractValidator<UpdateCartRequest>
    {
        public UpdateCartRequestValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.Date).NotEmpty();
            RuleForEach(x => x.Products).ChildRules(p =>
            {
                p.RuleFor(p => p.ProductId).GreaterThan(0);
                p.RuleFor(p => p.Quantity).GreaterThan(0);
            });
        }
    }
}
