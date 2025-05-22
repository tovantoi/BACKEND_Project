using chuyennganh.Application.App.CustomersApp.Customers.Command;
using FluentValidation;

namespace chuyennganh.Application.App.CustomersApp.Validators
{
    public class CustomerProfileValidator : AbstractValidator<UpdateProifleCustomerRequest>
    {
        public CustomerProfileValidator() { }
    }
}
