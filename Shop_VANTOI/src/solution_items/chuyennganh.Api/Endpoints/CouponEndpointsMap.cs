using chuyennganh.Api.Controllers;

namespace chuyennganh.Api.Endpoints
{
    public static class CouponEndpointsMap
    {
        public static IEndpointRouteBuilder MapCouponEndpoints(this IEndpointRouteBuilder app)
        {
            var customers = app.MapGroup("/minimal/api");

            customers.MapPost("/create-coupon", CouponController.CreateCoupon);
            customers.MapGet("/get-code-coupon", CouponController.GetByCodeCoupon);
            customers.MapPut("/update-coupon", CouponController.UpdateCoupon);
            customers.MapGet("/get-coupons", CouponController.GetAllCoupon);
            customers.MapGet("/get-coupon-by-id", CouponController.GetByIdCoupon);

            return app;
        }
    }
}
