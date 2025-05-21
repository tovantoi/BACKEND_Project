using chuyennganh.Api.Controllers;
using chuyennganh.Domain.Entities;

namespace chuyennganh.Api.Endpoints
{
    public static class ProductEndpointsMap
    {
        public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
        {
            var product = app.MapGroup("/minimal/api");
            product.MapPost("/create-product", ProductController.Post);
            product.MapDelete("delete-product", ProductController.Delete);
            product.MapPut("/update-product", ProductController.Put);
            product.MapGet("/get-name-product", ProductController.GetByNameProduct);
            product.MapGet("/get-product-by-id", ProductController.GetById); 
            product.MapGet("/get-products", ProductController.GetAll);
            product.MapGet("/get-products-admin", ProductController.GetAllProductAdmin);
            product.MapGet("/get-product-detail", ProductController.GetProductDetail);
            product.MapGet("/get-products-by-category", ProductController.GetAllProductByCategory);
            return app;
        }
    }
}
