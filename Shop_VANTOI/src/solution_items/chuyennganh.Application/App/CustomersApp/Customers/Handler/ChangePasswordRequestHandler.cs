using chuyennganh.Application.App.CustomersApp.Customers.Command;
using chuyennganh.Application.App.CustomersApp.Validators;
using chuyennganh.Application.Repositories;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.CustomersApp.Customers.Handler
{
    public class ChangePasswordRequestHandler : IRequestHandler<ChangePasswordRequest, ServiceResponse>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IEmailService emailService;

        public ChangePasswordRequestHandler(ICustomerRepository customerRepository, IEmailService emailService)
        {
            this.customerRepository = customerRepository;
            this.emailService = emailService;
        }

        public async Task<ServiceResponse> Handle(ChangePasswordRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = customerRepository.BeginTransaction())
            {
                try
                {
                    var validator = new ChangePasswordRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    var customer = await customerRepository.FindSingleAsync(x => x.Email == request.Email && x.IsActive);
                    if (customer is null) customer.ThrowNotFound();

                    var otp = Guid.NewGuid().ToString().Substring(0, 6).ToUpper();
                    customer.OTP = otp;
                    customer.OTPExpiration = DateTime.Now.AddSeconds(90);
                    await customerRepository.UpdateAsync(customer);
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
                                Cảm ơn bạn đã đăng ký tài khoản tại <strong>Fashionista</strong> - nơi mang đến những phong cách thời trang hiện đại và độc đáo. Để hoàn tất quá trình thay đổi mật khẩu, vui lòng sử dụng mã xác thực bên dưới:</p>
                                <div style='margin: 20px 0; padding: 15px; background-color: #fef8f9; border-radius: 8px; text-align: center; font-size: 24px; font-weight: bold; color: #FF4081;'>
                                    {otp}
                                </div>
                                <p>Nếu bạn không thực hiện yêu cầu này, vui lòng bỏ qua email này.</p>
                                <p>Trân trọng,<br/>Đội ngũ Fashionista!</p>
                            </div>
                            <div style='background-color: #FF4081; color: white; padding: 10px; text-align: center; font-size: 12px; border-top: 3px solid #FF4081;'>
                                © 2024 Fashionista - Định hình phong cách của bạn
                            </div>
                        </div>";
                    await emailService.SendEmailAsync(request.Email, subject, body);
                    await transaction.CommitAsync(cancellationToken);
                    return ServiceResponse.Success("Gửi mã OTP thành công");
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