using AutoMapper;
using chuyennganh.Application.App.CustomersApp.Customers.Command;
using chuyennganh.Application.App.CustomersApp.Validators;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Entities;
using MediatR;

namespace NhaThuoc.Application.Handlers.Customers
{
    public class RegisterRequestHandler : IRequestHandler<RegisterRequest, ServiceResponse>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;
        private readonly chuyennganh.Application.Repositories.IEmailService emailService;

        public RegisterRequestHandler(ICustomerRepository customerRepository, IMapper mapper, chuyennganh.Application.Repositories.IEmailService emailService)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
            this.emailService = emailService;
        }

        public async Task<ServiceResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = customerRepository.BeginTransaction())
            {
                try
                {
                    var validator = new RegisterRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    Customer? existingCustomer = null;                 

                    // Nếu là đăng ký truyền thống (không có GoogleCredential)
                    var otp = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
                    var existCustomer = await customerRepository.FindSingleAsync(x => x.Email == request.Email && !x.IsActive);

                    if (existCustomer is not null)
                    {
                        existCustomer.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                        existCustomer.OTP = otp;
                        existCustomer.OTPExpiration = DateTime.Now.AddSeconds(90);
                        await customerRepository.UpdateAsync(existCustomer);
                    }
                    else
                    {
                        var customer = new Customer
                        {
                            FirstName = request.FirstName!,
                            LastName = request.LastName!,
                            Email = request.Email!,
                            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                            OTP = otp,
                            OTPExpiration = DateTime.Now.AddSeconds(90),
                            Role = 0,
                            IsActive = false
                        };

                        await customerRepository.AddAsync(customer);
                    }

                    await customerRepository.SaveChangeAsync();

                    var subject = "Xác thực tài khoản - Fashionista!";
                    var body = $@"
                         <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; color: #333;'>
                             <div style='padding: 20px; border-bottom: 3px solid #FF4081; text-align: center;'>
                                <img src='https://drive.google.com/uc?export=view&id=1aOutIk00DMVmKlkNDRD8ID83L4I_d9KR' alt='Fashionista Logo' style='width: 150px; margin-bottom: 10px;border-radius: 20px;' />
                                 <h2 style='color: #FF4081; font-weight: bold; margin: 0;'>Chào mừng đến với Fashionista!</h2>
                             </div>
                             <div style='padding: 20px;'>
                                 <p>Xin chào,<br>
                                 Cảm ơn bạn đã đăng ký tài khoản tại <strong>Fashionista</strong> - nơi mang đến những phong cách thời trang hiện đại và độc đáo. Để hoàn tất quá trình đăng ký, vui lòng sử dụng mã xác thực bên dưới:</p>
                                 <div style='margin: 20px 0; padding: 15px; background-color: #fef8f9; border-radius: 8px; text-align: center; font-size: 24px; font-weight: bold; color: #FF4081;'>
                                     {otp}
                                 </div>
                                 <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>gg
                                 <p>Trân trọng,<br/>Đội ngũ Fashionista!</p>
                             </div>
                             <div style='background-color: #FF4081; color: white; padding: 10px; text-align: center; font-size: 12px; border-top: 3px solid #FF4081;'>
                                 © 2024 Fashionista - Định hình phong cách của bạn
                             </div>
                         </div>";

                    await emailService.SendEmailAsync(request.Email!, subject, body);
                    await transaction.CommitAsync();
                    return ServiceResponse.Success("Đăng ký thành công, vui lòng xác thực email.");
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