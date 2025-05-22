using chuyennganh.Application.App.OrderApp.Handler;
using chuyennganh.Application.Repositories;
using chuyennganh.Application.Repositories.BlogRepo;
using chuyennganh.Application.Repositories.CategoryRepo;
using chuyennganh.Application.Repositories.CouponRepo;
using chuyennganh.Application.Repositories.CustomerAddressRPRepo;
using chuyennganh.Application.Repositories.CustomerRPRepo;
using chuyennganh.Application.Repositories.OrderItemRepo;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Services;
using chuyennganh.Infrasture.Context;
using chuyennganh.Infrasture.Repositories;
using chuyennganh.Infrasture.Repositories.BlogRepo;
using chuyennganh.Infrasture.Repositories.CategoryRepo;
using chuyennganh.Infrasture.Repositories.CouponRepo;
using chuyennganh.Infrasture.Repositories.CustomerAddressRepo;
using chuyennganh.Infrasture.Repositories.CustomerRepo;
using chuyennganh.Infrasture.Repositories.OrderItemRepo;
using chuyennganh.Infrasture.Repositories.OrderRepo;
using chuyennganh.Infrasture.Repositories.ProductRepo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace chuyennganh.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfractureServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerAddressRepository, CustomerAddressRepository>();
            services.AddScoped<IProductReviewRepository, ProductReviewRepository>();
            services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IGoogleRepository, GoogleRepository>();
            services.AddScoped<SocialLoginService>();
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
          //  services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped(typeof(IGenericReponsitory<>), typeof(GenericRepository<>));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetByIdOrderRequestHandler).Assembly));
            //  services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<LoginCommandHandler>());
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddSingleton<EmailService>();
            return services;
        }
    }
}
