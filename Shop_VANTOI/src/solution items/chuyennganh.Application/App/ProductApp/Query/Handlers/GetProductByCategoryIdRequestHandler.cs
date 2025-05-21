using AutoMapper;
using chuyennganh.Application.App.DTOs;
using chuyennganh.Application.App.ProductApp.Query.Queries;
using chuyennganh.Application.Repositories.CategoryRepo;
using chuyennganh.Application.Repositories.OrderItemRepo;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.ExceptionEx;
using chuyennganh.Domain.Services;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Query.Handlers
{
    public class GetProductByCategoryIdRequestHandler : IRequestHandler<GetProductByCategoryIdRequest, PagedResponse<ProductDTO>>
    {
        private readonly IProductRepository productRepository;
        private readonly IProductCategoryRepository productCategoryRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMapper mapper;
        private readonly IOrderRepository orderRepository;
        private readonly IOrderItemRepository orderItemRepository;
        private readonly IFileService fileService;

        public GetProductByCategoryIdRequestHandler(IProductRepository productRepository, IMapper mapper, IProductCategoryRepository productCategoryRepository = null, ICategoryRepository categoryRepository = null, IOrderRepository orderRepository = null, IOrderItemRepository orderItemRepository = null, IFileService fileService = null)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.productCategoryRepository = productCategoryRepository;
            this.categoryRepository = categoryRepository;
            this.orderRepository = orderRepository;
            this.orderItemRepository = orderItemRepository;
            this.fileService = fileService;
        }
        public async Task<PagedResponse<ProductDTO>> Handle(GetProductByCategoryIdRequest request, CancellationToken cancellationToken)
        {
            var category = await categoryRepository.GetByIdAsync(request.CategoryId);

            if (category is null) category.ThrowNotFound();
            var categoryIds = productCategoryRepository.FindAll(x => x.CategoryId == request.CategoryId).Select(x => x.ProductId).ToList();
            var products = productRepository.FindAll(x => categoryIds.Contains(x.Id)).ToList();
            var employees = await Task.Run(() =>
            {
                return products
                    .Select(c => new
                    {
                        Product = c,
                        ImagePath = string.IsNullOrEmpty(c.ImagePath) ? null : fileService.GetFullPathFileServer(c.ImagePath)
                    })
                    .ToList();
            }, cancellationToken);

            var result = employees
                .Select(x =>
                {
                    x.Product.ImagePath = x.ImagePath;
                    return x.Product;
                })
                .ToList();
            var totalCount = products.Count();
            var pagedProducts = products.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToList();
            var productDtos = mapper.Map<List<ProductDTO>>(products);

            var successfulOrders = orderRepository.FindAll(x => x.Status == OrderStatus.Successed).ToList();
            var successfulOrderIds = successfulOrders.Select(o => o.Id).ToList();
            var orderItems = orderItemRepository.FindAll(x => successfulOrderIds.Contains(x.OrderId)).ToList();
            var productSaleCount = orderItems.GroupBy(item => item.ProductId).ToDictionary(group => group.Key, group => group.Sum(item => item.Quantity));

            foreach (var productDto in productDtos) productDto.AmountSeller = productSaleCount.GetValueOrDefault(productDto.Id, 0);
            return new PagedResponse<ProductDTO>(productDtos, request.PageNumber, request.PageSize, totalCount);
        }
    }
}
