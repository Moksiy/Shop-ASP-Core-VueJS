using FluentValidation;
using Shop.Application.ProductsAdmin;


namespace Shop.UI.ValidationContexts
{
    public class AddProductRequestValidation
        : AbstractValidator<CreateProduct.Request>
    {
        public AddProductRequestValidation()
        {
            RuleFor(x => x.Name).MinimumLength(3).NotNull();
            RuleFor(x => x.Description).MinimumLength(3).NotNull();
            RuleFor(x => x.Value).NotNull();
        }
    }
}
