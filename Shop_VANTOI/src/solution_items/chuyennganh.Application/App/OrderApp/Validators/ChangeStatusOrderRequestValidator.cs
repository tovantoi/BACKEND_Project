using chuyennganh.Application.App.OrderApp.Command;
using FluentValidation;

namespace chuyennganh.Application.App.OrderApp.Validators
{
    public class ChangeStatusOrderRequestValidator : AbstractValidator<ChangeStatusOrderRequest>
    {
        public ChangeStatusOrderRequestValidator()
        {
            RuleFor(c => c.Id)
                .NotNull().WithMessage("Id không được để trống.")
                .NotEmpty()
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }
}
