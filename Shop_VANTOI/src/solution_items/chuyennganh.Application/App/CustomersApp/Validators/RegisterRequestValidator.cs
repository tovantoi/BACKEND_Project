using chuyennganh.Application.App.CustomersApp.Customers.Command;
using FluentValidation;

namespace chuyennganh.Application.App.CustomersApp.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator() { }
    }
}
