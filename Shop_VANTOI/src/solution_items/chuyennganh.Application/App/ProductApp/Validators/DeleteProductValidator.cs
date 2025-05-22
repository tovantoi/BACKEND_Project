using chuyennganh.Application.App.ProductApp.Command;
using FluentValidation;

namespace chuyennganh.Application.App.ProductApp.Validators
{
    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductValidator()
        {
            RuleFor(c => c.Id)
                            .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }
}
