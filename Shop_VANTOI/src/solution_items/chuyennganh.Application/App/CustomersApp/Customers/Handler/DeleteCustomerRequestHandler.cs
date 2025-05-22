using AutoMapper;
using chuyennganh.Application.App.CategoryApp.Command;
using chuyennganh.Application.App.CustomersApp.Validators;
using chuyennganh.Application.Repositories.CategoryRepo;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Application.Response;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;

namespace chuyennganh.Application.App.CustomersApp.Customers.Handler
{
    public class DeleteCustomerRequestHandler : IRequestHandler<DeleteCustomerRequest, ServiceResponse>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;
        private readonly ILogger<DeleteCustomerRequestHandler> logger;

        public DeleteCustomerRequestHandler(IProductRepository productRepository, IMapper mapper, ILogger<DeleteCustomerRequestHandler> logger, ICategoryRepository categoryRepository, ICustomerRepository customerRepository)
        {
            this.mapper = mapper;
            this.logger = logger;
            this.customerRepository = customerRepository;
        }
        public async Task<ServiceResponse> Handle(DeleteCustomerRequest request, CancellationToken cancellationToken)
        {
            var response = new ServiceResponse();
            var validator = new DeleteCustomerRequestValidator();
            validator.ValidateAndThrow(request);
            await using (var transaction = customerRepository.BeginTransaction())
            {
                try
                {
                    var product = await customerRepository.GetByIdAsync(request.Id!.Value);
                    if (product == null)
                    {
                        response.IsSuccess = false;
                        response.Message = "Customer ID not found";
                        return response;
                    }
                    await customerRepository.DeleteAsync(request.Id.Value);
                    await customerRepository.SaveChangeAsync();

                    transaction.Commit();

                    response.IsSuccess = true;
                    response.Message = "Delete Successful";

                    return response;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    response.IsSuccess = false;
                    response.Message = $"An error occurred: {ex.Message}";
                    return response;
                }

            }
        }
    }
}
