using chuyennganh.Application.App.CategoryApp.Command;
using chuyennganh.Application.App.CustomersApp.CustomerAddress.Command;
using FluentValidation;

namespace chuyennganh.Application.App.CustomersApp.Validators
{
    internal class DeleteCustomerRequestValidator : AbstractValidator<DeleteCustomerRequest>
    {
        public DeleteCustomerRequestValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0).WithMessage("Id phải lớn hơn 0.");
        }
    }
}
