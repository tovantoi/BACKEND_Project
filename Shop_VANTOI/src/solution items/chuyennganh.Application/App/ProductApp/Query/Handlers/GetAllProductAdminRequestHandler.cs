using AutoMapper;
using chuyennganh.Application.App.DTOs;
using chuyennganh.Application.App.ProductApp.Query.Queries;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.Services;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Query.Handlers
{
    public class GetAllProductAdminRequestHandler : IRequestHandler<GetAllProductAdminQueris, List<ProductDTO>>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        public GetAllProductAdminRequestHandler(IProductRepository productRepository, IMapper mapper, IFileService fileService)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<List<ProductDTO>> Handle(GetAllProductAdminQueris request, CancellationToken cancellationToken)
        {
            var products = productRepository.FindAll();
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
            return mapper.Map<List<ProductDTO>>(products);
        }
    }
}
