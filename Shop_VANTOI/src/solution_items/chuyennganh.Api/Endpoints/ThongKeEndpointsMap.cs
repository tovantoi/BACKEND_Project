using chuyennganh.Api.Controllers;

namespace chuyennganh.Api.Endpoints
{
    public static class ThongKeEndpointsMap
    {
        public static IEndpointRouteBuilder MapThongKeEndpoints(this IEndpointRouteBuilder app)
        {
            var category = app.MapGroup("/minimal/api");

            category.MapGet("/get-top-product", ThongKeController.GetTopProduct);
            category.MapGet("/get-revenue-product-in-category", ThongKeController.GetRevenueCategory);
            category.MapGet("/get-revenue-day-week-month", ThongKeController.GetRevenueDayWeekMonth);
            category.MapGet("/get-sales-summary", ThongKeController.GetSalesSummary);
            return app;
        }
    }
}
