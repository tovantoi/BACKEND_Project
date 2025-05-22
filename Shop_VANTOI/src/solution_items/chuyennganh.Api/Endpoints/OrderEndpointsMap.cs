using chuyennganh.Api.Controllers;

namespace chuyennganh.Api.Endpoints
{
    public static class OrderEndpointsMap
    {
        public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
        {
            var customers = app.MapGroup("/minimal/api"); 

            customers.MapPost("/create-order", OrderController.CreateOrder);
            customers.MapGet("/get-orders", OrderController.GetAllOrder);
            customers.MapGet("/get-order-by-id", OrderController.GetOrderById);
            customers.MapGet("/get-order-by-customer-id", OrderController.GetOrderByCustomerId);
            customers.MapPut("/change-status-order", OrderController.UpdateOrder);

            return app;
        }
    }
}
