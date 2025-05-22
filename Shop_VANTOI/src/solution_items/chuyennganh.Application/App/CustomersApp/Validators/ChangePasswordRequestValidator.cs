using chuyennganh.Application.App.CustomersApp.Customers.Command;
using FluentValidation;

namespace chuyennganh.Application.App.CustomersApp.Validators
{
    public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
    {
        public ChangePasswordRequestValidator()
        {
            RuleFor(u => u.Email)
                .NotNull().WithMessage("Email không được để trống.")
                .NotEmpty().WithMessage("Email không được rỗng.")
                .EmailAddress().WithMessage("Email không đúng định dạng.")
                .MaximumLength(50).WithMessage("LastName không được vượt quá 450 ký tự.");
        }
    }
}