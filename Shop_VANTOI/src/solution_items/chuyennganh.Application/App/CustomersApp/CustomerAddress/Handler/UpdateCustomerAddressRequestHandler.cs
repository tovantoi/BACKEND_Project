using AutoMapper;
using chuyennganh.Application.App.CustomersApp.CustomerAddress.Command;
using chuyennganh.Application.App.CustomersApp.Validators;
using chuyennganh.Application.Repositories.CustomerAddressRPRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace NhaThuoc.Application.Handlers.CustomerAddress
{
    public class UpdateCustomerAddressRequestHandler : IRequestHandler<CustomerAddressUpdateRequest, ServiceResponse>
    {
        private readonly ICustomerAddressRepository customerAddressRepository;
        private readonly IMapper mapper;

        public UpdateCustomerAddressRequestHandler(ICustomerAddressRepository customerAddressRepository, IMapper mapper)
        {
            this.customerAddressRepository = customerAddressRepository;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse> Handle(CustomerAddressUpdateRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = customerAddressRepository.BeginTransaction())
            {
                try
                {
                    var validator = new UpdateCustomerAddressRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var customeraddress = await customerAddressRepository.GetByIdAsync(request.Id!);
                    if (customeraddress is null) customeraddress.ThrowNotFound();
                    customeraddress.CustomerId = request.CustomerId ?? customeraddress.CustomerId;
                    customeraddress.Address = request.Address ?? customeraddress.Address;
                    customeraddress.FullName = request.FullName ?? customeraddress.FullName;
                    customeraddress.Phone = request.Phone ?? customeraddress.Phone;
                    customeraddress.Province = request.Province ?? customeraddress.Province;
                    customeraddress.District = request.District ?? customeraddress.District;
                    customeraddress.Ward = request.Ward ?? customeraddress.Ward;


                    await customerAddressRepository.UpdateAsync(customeraddress);
                    await customerAddressRepository.SaveChangeAsync();
                    await transaction.CommitAsync(cancellationToken);
                    return ServiceResponse.Success("Cập nhật thành công");
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
