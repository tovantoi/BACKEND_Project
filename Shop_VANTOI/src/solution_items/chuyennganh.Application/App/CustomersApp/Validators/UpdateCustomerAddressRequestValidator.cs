using chuyennganh.Application.App.CustomersApp.CustomerAddress.Command;
using FluentValidation;

namespace chuyennganh.Application.App.CustomersApp.Validators
{
    public class UpdateCustomerAddressRequestValidator : AbstractValidator<CustomerAddressUpdateRequest>
    {
        public UpdateCustomerAddressRequestValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");

            RuleFor(c => c.CustomerId)
                .GreaterThan(0).WithMessage("CustomerId phải lớn hơn 0.");

            RuleFor(c => c.Address)
                .MaximumLength(400).WithMessage("Address không được vượt quá 400 ký tự.");

            RuleFor(c => c.FullName)
                .MaximumLength(2000).WithMessage("FullName không được vượt quá 2000 ký tự.");

            RuleFor(c => c.Phone)
                .MaximumLength(20).WithMessage("Phone không được vượt quá 20 ký tự.");

            RuleFor(c => c.Province)
                .MaximumLength(2000).WithMessage("Province không được vượt quá 2000 ký tự.");

            RuleFor(c => c.District)
                .MaximumLength(2000).WithMessage("District không được vượt quá 2000 ký tự.");

            RuleFor(c => c.Ward)
                .MaximumLength(2000).WithMessage("Ward không được vượt quá 2000 ký tự.");
        }
    }
}
