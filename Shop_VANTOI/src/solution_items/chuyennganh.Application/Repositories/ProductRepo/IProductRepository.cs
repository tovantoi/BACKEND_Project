using chuyennganh.Application.App.ProductApp.Command;
using chuyennganh.Domain.Entities;

namespace chuyennganh.Application.Repositories.ProductRepo
{
    public interface IProductRepository : IGenericReponsitory<Product>
    {
        Task<Product?> GetByIdWithImagesAsync(int id);
        Task<List<ProductInventoryDto>> GetProductInventoryAsync();

    }

}
