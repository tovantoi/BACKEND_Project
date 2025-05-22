using AutoMapper;
using chuyennganh.Application.App.CouponApp.Command;
using chuyennganh.Application.App.CouponApp.Query.Queries;
using chuyennganh.Application.App.ProductApp.Query.Queries;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;

namespace chuyennganh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {

        [HttpGet("/get-coupon-by-id")]
        public static async Task<IResult> GetByIdCoupon(int id, IMediator mediator)
        {
            var command = new GetByIdCouponRequest();
            command.Id = id;
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }

        [HttpGet("/get-coupons")]
        public static async Task<IResult> GetAllCoupon(IMediator mediator)
        {
            var command = new GetAllCouponRequest();
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }

        [HttpPost("/create-coupon")]
        public static async Task<IResult> CreateCoupon([FromBody] CouponCreateRequest request, IMediator mediator, IMapper mapper)
        {

            var command = mapper.Map<CouponCreateRequest>(request);
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }
        [HttpGet("/get-code-coupon")]
        public static async Task<IResult> GetByCodeCoupon(string couponcode, IMediator mediator)
        {
            var query = new GetByNameCouponRequest { Code = couponcode };
            var results = await mediator.Send(query);
            if (results != null)
            {
                return TypedResults.Ok(results);
            }
            return TypedResults.NotFound("Không có sản phẩm nào.");
        }

        [HttpPut("/update-coupon")]
        public static async Task<IResult> UpdateCoupon(int? id, [FromBody] CouponUpdateRequest request, IMediator mediator, IMapper mapper)
        {
            var command = mapper.Map<CouponUpdateRequest>(request);
            command.Id = id;
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return TypedResults.Ok(result);
            }
            return TypedResults.BadRequest(result);
        }
    }
}