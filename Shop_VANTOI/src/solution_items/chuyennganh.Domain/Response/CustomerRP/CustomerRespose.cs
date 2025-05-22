using chuyennganh.Domain.Entities;

namespace chuyennganh.Application.Response.UserRP
{
    public class CustomerRespose : ServiceResponse
    {
        public Customer Customer { get; }

        public CustomerRespose(Customer user)
        {
            Customer = user;
            IsSuccess = true;
        }
    }
}
