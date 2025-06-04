//using AutoMapper;
//using chuyennganh.Application.App.ProductApp.Query.Queries;
//using chuyennganh.Application.Repositories.ProductRepo;
//using chuyennganh.Application.Response;
//using chuyennganh.Domain.Abstractions;
//using chuyennganh.Domain.Entities;
//using chuyennganh.Domain.ExceptionEx;
//using MediatR;

//namespace chuyennganh.Application.App.ProductApp.Query.Handlers
//{
//    public class GetByIdImageProductRequestHandler : IRequestHandler<GetByIdImageProductQueris, List<ProductImage>>
//    {
//        private readonly IProductRepository productRepository;
//        private readonly IProductImageRepository productImageRepository;
//        private readonly IMapper mapper;
//        private readonly IFileService fileService;

//        public GetByIdImageProductRequestHandler(IProductRepository productRepository, IProductImageRepository productImageRepository, IMapper mapper, IFileService fileService)
//        {
//            this.productRepository = productRepository;
//            this.productImageRepository = productImageRepository;
//            this.mapper = mapper;
//            this.fileService = fileService;
//        }

//        public async Task<List<ProductImage>> Handle(GetByIdImageProductQueris request, CancellationToken cancellationToken)
//        {
//            var response = new ServiceResponse();
//            var product = productRepository.FindAll(x => x.ProductId!.ToLower().Contains(request.ProductId!)).ToList();
//            var employees = await Task.Run(() =>
//            {
//                return product
//                    .Select(c => new
//                    {
//                        Product = c,
//                        ImagePath = string.IsNullOrEmpty(c.ImagePath) ? null : fileService.GetFullPathFileServer(c.ImagePath)
//                    })
//                    .ToList();
//            }, cancellationToken);

//            var result = employees
//                .Select(x =>
//                {
//                    x.Product.ImagePath = x.ImagePath;
//                    return x.Product;
//                })
//                .ToList();
//            if (product is null) product.ThrowNotFound();
//            return mapper.Map<List<ProductImage>>(product);
//        }
//    }
//}
