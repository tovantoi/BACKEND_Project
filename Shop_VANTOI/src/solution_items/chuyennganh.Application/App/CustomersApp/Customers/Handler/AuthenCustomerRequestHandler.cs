using chuyennganh.Application.App.CustomersApp.Customers.Command;
using chuyennganh.Application.App.CustomersApp.Validators;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.CustomersApp.Customers.Handler
{
    public class AuthenCustomerRequestHandler : IRequestHandler<AuthenCustomerRequest, ServiceResponse>
    {
        private readonly ICustomerRepository customerRepository;

        public AuthenCustomerRequestHandler(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public async Task<ServiceResponse> Handle(AuthenCustomerRequest request, CancellationToken cancellationToken)
        {
            var response = new ServiceResponse();
            await using (var transaction = customerRepository.BeginTransaction())
            {
                try
                {
                    // Validate request
                    var validator = new AurhenCustomerRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    // Find customer by email and OTP
                    var customer = await customerRepository.FindSingleAsync(x => x.Email == request.Email && x.OTP == request.OTP);
                    if (customer == null) customer.ThrowNotFound();
               
                    // Check if OTP has expired
                    if (customer.OTPExpiration < DateTime.UtcNow) customer.ThrowConflict("OTP is expired!");
                    // Update customer status
                    customer.IsActive = true;
                    await customerRepository.UpdateAsync(customer); // Make sure to await the async method
                    await customerRepository.SaveChangeAsync();

                    // Commit transaction
                    await transaction.CommitAsync(cancellationToken);

                    return ServiceResponse.Success("Xác nhận OTP thành công.");
                }
                catch (System.Exception ex)
                {
                    // Rollback the transaction if any error occurs
                    await transaction.RollbackAsync(cancellationToken);

                    // Optionally log the exception (for example with a logger)
                    return ServiceResponse.Failure($"An error occurred: {ex.Message}"); 
                }
            }
        }
    }
}
