using AutoMapper;
using chuyennganh.Application.App.CustomersApp.CustomerAddress.Command;
using chuyennganh.Application.App.CustomersApp.Validators;
using chuyennganh.Application.Repositories.CustomerAddressRPRepo;
using chuyennganh.Application.Response;
using MediatR;
using Entities = chuyennganh.Domain.Entities;

namespace NhaThuoc.Application.Handlers.CustomerAddress
{
    public class CreateCustomerAddressRequestHandler : IRequestHandler<CustomerAddressCreateRequest, ServiceResponse>
    {
        private readonly ICustomerAddressRepository customerAddressRepository;
        private readonly IMapper mapper;

        public CreateCustomerAddressRequestHandler(ICustomerAddressRepository customerAddressRepository, IMapper mapper)
        {
            this.customerAddressRepository = customerAddressRepository;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse> Handle(CustomerAddressCreateRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = customerAddressRepository.BeginTransaction())
            {
                try
                {
                    var validator = new CreateCustomerAddressRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var customeraddress = mapper.Map<Entities.CustomerAddress>(request);

                    await customerAddressRepository.AddAsync(customeraddress);
                    await customerAddressRepository.SaveChangeAsync();
                    var createdAddressId = customeraddress.Id;
                    await transaction.CommitAsync(cancellationToken);
                    return ServiceResponse.Success("Tạo địa chỉ thành công", query: new { Id = createdAddressId });
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
