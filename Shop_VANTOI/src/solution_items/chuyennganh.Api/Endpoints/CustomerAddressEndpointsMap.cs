using chuyennganh.Api.Controllers;

namespace chuyennganh.Api.Endpoints
{
    public static class CustomerAddressEndpointsMap
    {
        public static IEndpointRouteBuilder MapCustomerAddressEndpoints(this IEndpointRouteBuilder app)
        {
            var customers = app.MapGroup("/minimal/api");

            customers.MapPost("/create-customeraddress", CustomersAddressController.CreateCustomerAddress);
            customers.MapGet("/get-address-by-customer-id", CustomersAddressController.GetCustomerAddressByCustomerId);
            customers.MapPut("/update-customeraddress", CustomersAddressController.UpdateCustomerAddress);
            customers.MapGet("/get-customeraddresss", CustomersAddressController.GetAllCustomerAddress);
            customers.MapGet("/get-customeraddress-by-id", CustomersAddressController.GetByIdCustomerAddress);

            return app;
        }
    }
}
