using chuyennganh.Application.App.CustomersApp.CustomerAddress.Command;
using FluentValidation;

namespace chuyennganh.Application.App.CustomersApp.Validators
{
    public class CreateCustomerAddressRequestValidator : AbstractValidator<CustomerAddressCreateRequest>
    {
        public CreateCustomerAddressRequestValidator()
        {
            RuleFor(c => c.CustomerId)
                .NotNull().WithMessage("CustomerId không được để trống.")
                .GreaterThan(0).WithMessage("CustomerId phải lớn hơn 0.");

            RuleFor(c => c.Address)
                .NotNull().WithMessage("Address không được để trống.")
                .NotEmpty().WithMessage("Address không được rỗng.")
                .MaximumLength(400).WithMessage("Address không được vượt quá 400 ký tự.");

            RuleFor(c => c.FullName)
                .NotNull().WithMessage("FullName không được để trống.")
                .NotEmpty().WithMessage("FullName không được rỗng.")
                .MaximumLength(2000).WithMessage("FullName không được vượt quá 2000 ký tự.");

            RuleFor(c => c.Phone)
                .NotNull().WithMessage("Phone không được để trống.")
                .NotEmpty().WithMessage("Phone không được rỗng.")
                .MaximumLength(20).WithMessage("Phone không được vượt quá 20 ký tự.");

            RuleFor(c => c.Province)
                .NotNull().WithMessage("Province không được để trống.")
                .NotEmpty().WithMessage("Province không được rỗng.")
                .MaximumLength(2000).WithMessage("Province không được vượt quá 2000 ký tự.");

            RuleFor(c => c.District)
                .NotNull().WithMessage("District không được để trống.")
                .NotEmpty().WithMessage("District không được rỗng.")
                .MaximumLength(2000).WithMessage("District không được vượt quá 2000 ký tự.");

            RuleFor(c => c.Ward)
                .NotNull().WithMessage("Ward không được để trống.")
                .NotEmpty().WithMessage("Ward không được rỗng.")
                .MaximumLength(2000).WithMessage("Ward không được vượt quá 2000 ký tự.");
        }
    }
}
