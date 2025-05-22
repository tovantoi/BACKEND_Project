using chuyennganh.Domain.Entities;

namespace chuyennganh.Application.Response.ProductsRP
{
    public class ProductRespose : ServiceResponse
    {
        public Product Product { get; }

        public ProductRespose(Product product)
        {
            Product = product;
            IsSuccess = true;
        }
    }
}
