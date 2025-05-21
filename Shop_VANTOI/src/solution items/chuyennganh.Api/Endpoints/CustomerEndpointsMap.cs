using chuyennganh.Api.Controllers;

namespace chuyennganh.Api.Endpoints
{
    public static class CustomerEndpointsMap
    {
        public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            var customers = app.MapGroup("/minimal/api");

            customers.MapPost("/register-customer", CustomersController.RegisterCustomer);
            customers.MapPost("/register-google", CustomersController.RegisterGoogle);
            customers.MapPost("/login-customer", CustomersController.Login);
            customers.MapPut("/authen-customer", CustomersController.AuthenCustomer);
            customers.MapPost("/resend-otp", CustomersController.ResendOTP);
            customers.MapPost("/customer-logout", CustomersController.Logout);
            customers.MapGet("/get-customer-by-id", CustomersController.GetCustomerById); 
            customers.MapGet("/get-customers", CustomersController.GetAllCustomerRoles); 
            customers.MapPut("/change-password", CustomersController.ChangePassword);
            customers.MapPut("/update-customer-password", CustomersController.UpdateCustomerPassword); 
            customers.MapPut("/update-profile-customer", CustomersController.UpdateCustomerProfile);
            customers.MapDelete("/delete-customer", CustomersController.Delete);
            return app;
        }
    }
}
