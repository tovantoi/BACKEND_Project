using chuyennganh.Application.App.ProductApp.Command;
using chuyennganh.Application.Repositories.ProductRepo;
using MediatR;

namespace chuyennganh.Application.App.ProductApp.Query.Handlers
{
    public class GetProductInventoryQuery : IRequest<List<ProductInventoryDto>>
    {
    }

    public class GetProductInventoryHandler : IRequestHandler<GetProductInventoryQuery, List<ProductInventoryDto>>
    {
        private readonly IProductRepository _productRepository;
        public GetProductInventoryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductInventoryDto>> Handle(GetProductInventoryQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.GetProductInventoryAsync();
        }
    }
}
