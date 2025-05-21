using chuyennganh.Api.Controllers;

namespace chuyennganh.Api.Endpoints
{
    public static class ProductReviewEndpointsMap
    {
        public static IEndpointRouteBuilder MapProductReviewEndpoints(this IEndpointRouteBuilder app)
        {
            var productReview = app.MapGroup("/minimal/api");

            productReview.MapPost("/create-product-review", ProductReviewController.PostReview);
            productReview.MapGet("/get-product-review-by-id", ProductReviewController.GetByIdReview);
            productReview.MapGet("/get-product-review", ProductReviewController.GetAllReview);
            productReview.MapGet("/get-product-cmt-start", ProductReviewController.GetCmtStart);
            //productReview.MapDelete("/delete-product-review", ProductReviewController.Delete);
            productReview.MapPut("/update-product-review", ProductReviewController.PutReview);
            return app;
        }
    }
}
