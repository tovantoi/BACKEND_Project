namespace chuyennganh.Domain.Abstractions
{
    public interface IPaymentService
    {
        Task<string> CreatePayPalPayment(decimal amount, string orderId, string returnUrl);
    }
}
