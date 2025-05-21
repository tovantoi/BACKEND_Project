using AutoMapper;
using chuyennganh.Application.App.ProductApp.Query.Queries;
using chuyennganh.Application.Repositories.ProductRepo;
using chuyennganh.Application.Response;
using chuyennganh.Domain.Abstractions;
using chuyennganh.Domain.Entities;
using chuyennganh.Domain.ExceptionEx;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Query.Handlers
{
    public class GetByNameProductRequestHandler : IRequestHandler<GetByNameProductQueris, List<Product>>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;

        public GetByNameProductRequestHandler(IProductRepository productRepository, IMapper mapper, IFileService fileService)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }
        public async Task<List<Product>> Handle(GetByNameProductQueris request, CancellationToken cancellationToken)
        {
            var response = new ServiceResponse();
            var product = productRepository.FindAll(x => x.ProductName!.ToLower().Contains(request.ProductName!.ToLower())).ToList();
            var employees = await Task.Run(() =>
            {
                return product
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
            if (product is null) product.ThrowNotFound();
            return mapper.Map<List<Product>>(product);
        }
    }
}
