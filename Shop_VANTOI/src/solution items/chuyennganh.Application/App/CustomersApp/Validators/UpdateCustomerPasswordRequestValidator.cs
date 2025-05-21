using chuyennganh.Application.App.CustomersApp.Customers.Command;
using FluentValidation;

namespace chuyennganh.Application.App.CustomersApp.Validators
{
    public class UpdateCustomerPasswordRequestValidator : AbstractValidator<UpdateCustomerPasswordRequest>
    {
        public UpdateCustomerPasswordRequestValidator()
        {
            RuleFor(u => u.Email)
               .NotNull().WithMessage("Email không được để trống.")
               .NotEmpty().WithMessage("Email không được rỗng.")
               .EmailAddress().WithMessage("Email không đúng định dạng.")
               .MaximumLength(50).WithMessage("LastName không được vượt quá 450 ký tự.");

            RuleFor(u => u.NewPassword)
                .NotNull().WithMessage("Password không được để trống.")
                .NotEmpty().WithMessage("Password không được rỗng.")
                .MaximumLength(100).WithMessage("Password không vượt quá 100 ký tự.");

            RuleFor(u => u.ConfirmPassword)
                .NotNull().WithMessage("ConfirmPassword không được để trống.")
                .NotEmpty().WithMessage("ConfirmPassword không được rỗng.")
                .Equal(u => u.NewPassword).WithMessage("ConfirmPassword phải trùng với Password.");

            RuleFor(x => x.OTP).Length(6).WithMessage("OTP phải đủ 6 ký tự");
        }
    }
}