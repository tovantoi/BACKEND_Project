using chuyennganh.Application.App.PaymentTransaction.Command;
using FluentValidation;

namespace chuyennganh.Application.App.PaymentTransaction.Validators
{
    public class CreatePaymentTransactionValidator : AbstractValidator<CreatePaymentTransactionRequest>
    {
        public CreatePaymentTransactionValidator() { }
    }
}
