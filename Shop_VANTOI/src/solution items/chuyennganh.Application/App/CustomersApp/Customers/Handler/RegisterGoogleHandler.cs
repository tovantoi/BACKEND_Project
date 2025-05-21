using chuyennganh.Application.App.CustomersApp.Customers.Command;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.Services;
using MediatR;

namespace chuyennganh.Application.App.CustomersApp.Customers.Handler
{
    public class RegisterGoogleHandler : IRequestHandler<GoogleRegisterRequest, ServiceResponse>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IGoogleRepository googleRepository;
        private readonly SocialLoginService socialLoginService;

        public RegisterGoogleHandler(ICustomerRepository customerRepository, IGoogleRepository googleRepository, SocialLoginService socialLoginService)
        {
            this.customerRepository = customerRepository;
            this.googleRepository = googleRepository;
            this.socialLoginService = socialLoginService;
        }

        public async Task<ServiceResponse> Handle(GoogleRegisterRequest request, CancellationToken cancellationToken)
        {
            await using var transaction = customerRepository.BeginTransaction();

            try
            {
                var (success, email, name, givenName, familyName, picture, subject) =
                await socialLoginService.VerifyGoogleTokenAsync(request.GoogleCredential);

                if (!success)
                {
                    return ServiceResponse.Failure("Xác thực token Google thất bại.");
                }
                var nameParts = name?.Split(' ', 2);
                var firstName = nameParts?.FirstOrDefault() ?? "Google";
                var lastName = nameParts?.Skip(1).FirstOrDefault() ?? "User";


                // 1. Kiểm tra xem đã tồn tại customer nào với email này chưa
                var existingCustomer = await customerRepository.FindSingleAsync(x => x.Email == email);

                if (existingCustomer is not null)
                {
                    // Nếu đã có nhưng chưa kích hoạt, kích hoạt lại
                    if (!existingCustomer.IsActive)
                    {
                        existingCustomer.IsActive = true;
                        await customerRepository.UpdateAsync(existingCustomer);
                    }

                    await customerRepository.SaveChangeAsync();
                    await transaction.CommitAsync();
                    return ServiceResponse.Success("Đăng nhập bằng Google thành công");
                }

                // 2. Nếu chưa có -> tạo customer và liên kết GoogleAccount
                var newCustomer = new Customer
                {
                    FirstName = givenName ?? "Google",
                    LastName = familyName ?? "User",
                    Email = email,
                    IsActive = true,
                    Role = 0,
                    Password = "",        // để trống hoặc null nếu nullable
                    OTP = null,
                    OTPExpiration = null
                };

                await customerRepository.AddAsync(newCustomer);
                await customerRepository.SaveChangeAsync();

                // 3. Lưu GoogleAccount
                var googleAccount = new GoogleAccount
                {
                    GoogleId = subject,
                    Email = email,
                    FirstName = givenName,
                    LastName = familyName,
                    AvatarUrl = picture,
                };

                await googleRepository.AddAsync(googleAccount);
                await googleRepository.SaveChangeAsync();

                await transaction.CommitAsync();
                return ServiceResponse.Success("Tài khoản Google đã được tạo thành công");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(cancellationToken);
                return ServiceResponse.Failure("Lỗi khi đăng ký bằng Google: " + ex.Message);
            }
        }
    }
}
