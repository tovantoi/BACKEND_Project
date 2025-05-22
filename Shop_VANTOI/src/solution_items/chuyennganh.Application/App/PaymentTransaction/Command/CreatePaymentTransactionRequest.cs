using chuyennganh.Application.Response;
using MediatR;

namespace chuyennganh.Application.App.PaymentTransaction.Command
{
    public record CreatePaymentTransactionRequest : IRequest<ServiceResponse>
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = null!; 
        public string? ReturnUrl { get; set; }
        public string? TransactionId { get; set; }

    }
}
