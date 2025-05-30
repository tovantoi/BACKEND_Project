using chuyennganh.Api.Controllers;

namespace chuyennganh.Api.Endpoints
{
    public static class SendOrderEmailEndpointMap
    {
        public static IEndpointRouteBuilder MapSendOrderEmail(this IEndpointRouteBuilder app)
        {
            var sendOrder = app.MapGroup("/minimal/api");

            sendOrder.MapPost("/send-order-email", SendOrderEmailController.SendOrderEmail);

            return app;
        }
    }
}
