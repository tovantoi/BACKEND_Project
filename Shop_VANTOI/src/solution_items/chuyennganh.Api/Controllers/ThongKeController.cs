using AutoMapper;
using chuyennganh.Application.App.Thong_Ke.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace chuyennganh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ThongKeController : ControllerBase
    {
        [HttpGet("/get-top-product")]//Top sản phẩm bán chạy theo doanh thu
        public static async Task<IResult> GetTopProduct(IMediator mediator, IMapper mapper)
        {
            var command = new GetTopProductRevenueRequest();
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }

        [HttpGet("/get-sales-summary")]
                                public static async Task<IResult> GetSalesSummary(
                            [FromQuery] DateTime? from,
                            [FromQuery] DateTime? to,
                            IMediator mediator)
        {
            var query = new GetSalesSummaryRequest
            {
                From = from,
                To = to
            };

            var result = await mediator.Send(query);
            return TypedResults.Ok(result);
        }

        [HttpGet("/get-revenue-product-in-category")]// Thống kê doanh thu theo danh mục sản phẩm
        public static async Task<IResult> GetRevenueCategory(IMediator mediator, IMapper mapper)
        {
            var command = new GetCategoryRevenueRequest();
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }

        [HttpGet("/get-revenue-day-week-month")] // Thống kê doanh thu theo ngày tuần tháng năm
        public static async Task<IResult> GetRevenueDayWeekMonth(
                                                     [FromQuery] string? mode,
                                                     [FromQuery] DateTime? from,
                                                     [FromQuery] DateTime? to,
                                                     IMediator mediator)
        {
            var request = new GetRevenueStatisticsRequest
            {
                Mode = mode,
                From = from,
                To = to
            };

            var result = await mediator.Send(request);
            return TypedResults.Ok(result);
        }

    }
}
