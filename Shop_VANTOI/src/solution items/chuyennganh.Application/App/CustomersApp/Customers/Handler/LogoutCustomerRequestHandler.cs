using chuyennganh.Application.App.CustomersApp.Customers.Command;
using chuyennganh.Application.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace chuyennganh.Application.App.CustomersApp.Customers.Handler
{
    public class LogoutCustomerRequestHandler : IRequestHandler<LogoutCustomerRequest, ServiceResponse>
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public LogoutCustomerRequestHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<ServiceResponse> Handle(LogoutCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                httpContextAccessor!.HttpContext!.Session.Clear();
                httpContextAccessor.HttpContext.Response.Cookies.Delete(".AspNetCore.Session");
                return ServiceResponse.Success("Đăng xuất thành công");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}