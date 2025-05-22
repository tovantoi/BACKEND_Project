using chuyennganh.Api.Controllers;

namespace chuyennganh.Api.Endpoints
{
    public static class BlogEndpointsMap
    {
        public static IEndpointRouteBuilder MapBlogEndpoints(this IEndpointRouteBuilder app)
        {
            var category = app.MapGroup("/minimal/api");

            category.MapPost("/create-blog", BlogController.CreateBlog);
            category.MapGet("/get-blogs", BlogController.GetAllBlog);
            category.MapGet("/get-detail-blog", BlogController.GetDetailBlog);
            category.MapPut("/delete-blog", BlogController.DeleteBlog);
            category.MapPut("/update-blog", BlogController.UpdateBlog);
            return app;
        }
    }
}
