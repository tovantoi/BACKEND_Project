using chuyennganh.Application.App.CustomersApp.Customers.Command;
using chuyennganh.Application.App.CustomersApp.Validators;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.CustomersApp.Customers.Handler
{
    public class UpdateCustomerPasswordRequestHandler : IRequestHandler<UpdateCustomerPasswordRequest, ServiceResponse>
    {
        private readonly ICustomerRepository customerRepository;

        public UpdateCustomerPasswordRequestHandler(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task<ServiceResponse> Handle(UpdateCustomerPasswordRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = customerRepository.BeginTransaction())
            {
                try
                {
                    var validator = new UpdateCustomerPasswordRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var customer = await customerRepository.FindSingleAsync(x => x.Email == request.Email && x.OTP == request.OTP);
                    if (customer is null) customer.ThrowConflict("OTP không hợp lệ!");

                    if (customer.OTPExpiration < DateTime.UtcNow) customer.ThrowConflict("OTP đã quá hạn!");

                    customer.Password = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
                    await customerRepository.UpdateAsync(customer);
                    await customerRepository.SaveChangeAsync();

                    await transaction.CommitAsync(cancellationToken);
                    return ServiceResponse.Success("Cập nhật mật khẩu thành công");
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }
    }
}