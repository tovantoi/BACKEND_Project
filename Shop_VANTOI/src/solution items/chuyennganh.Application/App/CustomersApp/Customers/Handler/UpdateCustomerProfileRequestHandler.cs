using AutoMapper;
using chuyennganh.Application.App.CustomersApp.Customers.Command;
using chuyennganh.Application.App.CustomersApp.Validators;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.ExceptionEx;
using chuyennganh.Domain.Shared;
using MediatR;


namespace chuyennganh.Application.App.CustomersApp.Customers.Handler
{
    public class UpdateCustomerProfileRequestHandler : IRequestHandler<UpdateProifleCustomerRequest, ServiceResponse>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public UpdateCustomerProfileRequestHandler(ICustomerRepository customerRepository, IMapper mapper, IFileService fileService)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<ServiceResponse> Handle(UpdateProifleCustomerRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = customerRepository.BeginTransaction())
            {
                try
                {
                    var validator = new CustomerProfileValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var customer = await customerRepository.GetByIdAsync(request.Id!);
                    if (customer is null) customer.ThrowNotFound();
                    customer.FirstName = request.FirstName ?? customer.FirstName;
                    customer.LastName = request.LastName ?? customer.LastName;
                    customer.PhoneNumber = request.PhoneNumber ?? customer.PhoneNumber;
                    if (request.ImageData is not null)
                    {
                        var uploadFile = new UploadFileRequest
                        {
                            Content = request.ImageData,
                            AssetType = AssetType.Customer,
                            Suffix = customer.Id.ToString(),
                        };
                        customer.AvatarImagePath = await fileService.UploadFileAsync(uploadFile);
                    }
                    await customerRepository.UpdateAsync(customer);
                    await customerRepository.SaveChangeAsync();
                    await transaction.CommitAsync(cancellationToken);
                    return ServiceResponse.Success("Cập nhật thông tin thành công");
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
