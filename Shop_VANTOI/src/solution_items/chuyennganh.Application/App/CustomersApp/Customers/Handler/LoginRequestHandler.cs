using AutoMapper;
using chuyennganh.Application.App.CustomersApp.Customers.Command;
using chuyennganh.Application.App.CustomersApp.Validators;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.ExceptionEx;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace chuyennganh.Application.App.CustomersApp.Customers.Handler
{
    public class LoginRequestHandler : IRequestHandler<LoginRequest, ServiceResponse>
    {
        private readonly ICustomerRepository customerRepository;
        private readonly IMapper mapper;

        public LoginRequestHandler(ICustomerRepository customerRepository, IMapper mapper)
        {
            this.customerRepository = customerRepository;
            this.mapper = mapper;
        }

        public async Task<ServiceResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            await using (var transaction = customerRepository.BeginTransaction())
            {
                try
                {
                    var validator = new LoginRequestValidator();
                    var validationResult = await validator.ValidateAsync(request, cancellationToken);

                    // Tìm khách hàng theo email, đảm bảo chỉ có 1 khách hàng duy nhất
                    var customer = await customerRepository.FindAll(u => u.Email == request.Email).SingleOrDefaultAsync(cancellationToken);

                    // Nếu không tìm thấy khách hàng
                    if (customer is null)
                    {
                        return new ServiceResponse
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status404NotFound,
                            Message = "Khách hàng không tồn tại"
                        };
                    }

                    // Kiểm tra trạng thái kích hoạt
                    if (!customer.IsActive)
                    {
                        return new ServiceResponse
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status403Forbidden,
                            Message = "Tài khoản của bạn đã bị khóa"
                        };
                    }

                    // Kiểm tra mật khẩu
                    bool isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, customer.Password);
                    if (!isPasswordValid)
                    {
                        return new ServiceResponse
                        {
                            IsSuccess = false,
                            StatusCode = StatusCodes.Status401Unauthorized,
                            Message = "Mật khẩu không đúng"
                        };
                    }

                    // Commit transaction nếu không có lỗi
                    await transaction.CommitAsync(cancellationToken);

                    // Trả về thông báo thành công cùng thông tin người dùng (tuỳ chọn)
                    return new ServiceResponse
                    {
                        IsSuccess = true,
                        Message = "Đăng nhập thành công",
                        Query = new
                        {
                            customer.Id,
                            customer.FirstName, 
                            customer.LastName,  
                            customer.Email,
                            customer.Role,
                            customer.AvatarImagePath, 
                            customer.IsActive, 
                        }
                    };
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    // Log lỗi chi tiết để kiểm tra sau này
                    throw new ApplicationException("Lỗi trong quá trình đăng nhập: " + ex.Message);
                }
            }
        }
    }

}
