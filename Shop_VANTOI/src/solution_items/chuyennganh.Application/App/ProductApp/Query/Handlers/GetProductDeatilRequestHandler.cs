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
    public class GetProductDeatilRequestHandler : IRequestHandler<GetProductDeatilRequest, Product>
    {
        private readonly IProductRepository productRepository;
        private readonly IMapper mapper;
        private readonly IFileService fileService;
        public GetProductDeatilRequestHandler(IProductRepository productRepository, IMapper mapper, IFileService fileService)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.fileService = fileService;
        }

        public async Task<Product> Handle(GetProductDeatilRequest request, CancellationToken cancellationToken)
        {
            var response = new ServiceResponse();
            Product? product = await productRepository.GetByIdAsync(request.Id);
            if (product != null)
            {
                product.ImagePath = string.IsNullOrEmpty(product.ImagePath) ? null : fileService.GetFullPathFileServer(product.ImagePath); // Adding EmpImage property
            }
            if (product is null) product.ThrowNotFound();
            return mapper.Map<Product>(product);
        }
    }
}
