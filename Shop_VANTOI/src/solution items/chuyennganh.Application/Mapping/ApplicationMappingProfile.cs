using AutoMapper;
using chuyennganh.Application.App.Blogs.Command;
using chuyennganh.Application.App.CategoryApp.Command;
using chuyennganh.Application.App.CategoryApp.Validators;
using chuyennganh.Application.App.CouponApp.Command;
using chuyennganh.Application.App.CustomersApp.CustomerAddress.Command;
using chuyennganh.Application.App.CustomersApp.Customers.Command;
using chuyennganh.Application.App.DTOs;
using chuyennganh.Application.App.PaymentTransaction.Command;
using chuyennganh.Application.App.ProdcutReview.Command;
using chuyennganh.Application.App.ProductApp.Command;
using chuyennganh.Application.App.ProductApp.Validators;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Entities;

namespace chuyennganh.Application.Mapping
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            #region Product
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<CreateProductCommand, CreateProductValidator>().ReverseMap();
            CreateMap<Product, ServiceResponse>().ReverseMap();
            CreateMap<DeleteProductCommand, Product>().ReverseMap();
            CreateMap<DeleteProductCommand, ServiceResponse>().ReverseMap();
            CreateMap<UpdateProductCommand, Product>().ReverseMap()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<UpdateProductCommand, UpdateProductValidator>().ReverseMap();
            #endregion
            #region Category
            CreateMap<CreateCategoryRequest, Category>();
            CreateMap<CreateCategoryRequest, CreateCategoryRequestValidator>().ReverseMap();
            CreateMap<Category, ServiceResponse>().ReverseMap();
            CreateMap<DeleteCategoryRequest, Category>().ReverseMap();
            CreateMap<DeleteCategoryRequest, ServiceResponse>().ReverseMap();
            CreateMap<UpdateCategoryRequest, Category>();
            CreateMap<UpdateCategoryRequest, UpdateCategoryRequestValidator>().ReverseMap();
            #endregion
            #region Order
            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.CustomerAddress))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems))
                .ForMember(dest => dest.Coupon, opt => opt.MapFrom(src => src.Coupon))
                .ReverseMap()
                .ForMember(dest => dest.CustomerAddress, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.Coupon, opt => opt.MapFrom(src => src.Coupon))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
            CreateMap<CustomerAddress, CustomerAddressDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            #endregion
            #region Coupon
            CreateMap<Coupon, CouponCreateRequest>().ReverseMap();
            CreateMap<Coupon, CouponUpdateRequest>().ReverseMap();
            CreateMap<Coupon, CouponDTO>().ReverseMap();
            #endregion

            #region CustomerAddress
            CreateMap<CustomerAddress, CustomerAddressCreateRequest>().ReverseMap();
            CreateMap<CustomerAddress, CustomerAddressUpdateRequest>().ReverseMap();
            CreateMap<CustomerAddress, CustomerAddressDTO>().ReverseMap();
            #endregion
            #region Customer
            CreateMap<Customer, LoginRequest>().ReverseMap();
            CreateMap<Customer, RegisterRequest>().ReverseMap();
            CreateMap<GoogleAccount, GoogleRegisterRequest>().ReverseMap();
            CreateMap<Customer, AuthenCustomerRequest>().ReverseMap();
            // CreateMap<Customer, ChangePasswordRequest>().ReverseMap();
            // CreateMap<Customer, UpdateCustomerPasswordRequest>().ReverseMap();
            #endregion

            #region ProductReview
            CreateMap<CreateReviewRequest, ProductReview>();
            CreateMap<UpdateReviewRequest, ProductReview>();
            #endregion

            #region Blog
            CreateMap<CreateBlogRequest, Blog>();
            CreateMap<UpdateBlogRequest, Blog>();
            CreateMap<DeleteBlogRequest, ServiceResponse>();
            CreateMap<ProductReview, GetDetailReviewRequest>();
            #endregion

            #region PaymentTransaction
            CreateMap<CreatePaymentTransactionRequest, PaymentTransaction>();
            #endregion
        }
    }
}
