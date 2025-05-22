using chuyennganh.Application.App.CustomersApp.CustomerAddress.Query.Queries;
using FluentValidation;

namespace chuyennganh.Application.App.CustomersApp.Validators
{
    public class GetCustomerAddressByCustomerIdRequestValidator : AbstractValidator<GetCustomerAddressByCustomerIdRequest>
    {
        public GetCustomerAddressByCustomerIdRequestValidator()
        {
            RuleFor(o => o.CustomerId)
                .NotNull().WithMessage("CustomerId không được để trống.")
                .GreaterThan(0).WithMessage("CustomerId phải lớn hơn 0.");
        }
    }
}