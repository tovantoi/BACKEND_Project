using chuyennganh.Api.Controllers;

namespace chuyennganh.Api.Endpoints
{
    public static class PaymentEndpointsMap
    {
        public static IEndpointRouteBuilder MapPaymentEndpoints(this IEndpointRouteBuilder app)
        {
            var category = app.MapGroup("/minimal/api");

            category.MapPost("/create-payment", PaymentController.CreatePayment);
            return app;
        }
    }
}
