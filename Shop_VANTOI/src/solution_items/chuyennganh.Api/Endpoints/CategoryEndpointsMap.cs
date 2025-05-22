using chuyennganh.Api.Controllers;

namespace chuyennganh.Api.Endpoints
{
    public static class CategoryEndpointsMap
    {
        public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            var category = app.MapGroup("/minimal/api");

            category.MapPost("/create-category", CategoryController.CreateCategory);
            category.MapGet("/get-category-by-id", CategoryController.GetByIdCategory);
            category.MapGet("/get-categories", CategoryController.GetAllCategory);
            category.MapGet("/get-name-categories", CategoryController.GetAllNameCategory);
            category.MapDelete("/delete-category", CategoryController.Delete);
            category.MapPut("/update-category", CategoryController.UpdateCategory);
            return app;
        }
    }
}
