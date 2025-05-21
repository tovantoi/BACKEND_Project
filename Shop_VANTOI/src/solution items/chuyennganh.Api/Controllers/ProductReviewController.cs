using AutoMapper;
using chuyennganh.Application.App.ProdcutReview.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace chuyennganh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        [HttpPost("/create-product-review")]
        public static async Task<IResult> PostReview([FromBody] CreateReviewRequest request, IMediator mediator)
        {
            var results = await mediator.Send(request);
            if (results.IsSuccess)
            {
                return TypedResults.Ok(results);
            }
            return TypedResults.BadRequest(results);
        }
        [HttpPut("/update-product-review")]
        public static async Task<IResult> PutReview(int id, [FromBody] UpdateReviewRequest request, IMediator mediator)
        {
            request.Id = id;
            var results = await mediator.Send(request);
            if (results.IsSuccess)
            {
                return TypedResults.Ok(results);
            }
            return TypedResults.BadRequest(results);
        }
        [HttpGet("/get-product-review")]
        public static async Task<IResult> GetAllReview(IMediator mediator)
        {
            var command = new GetAllReviewRequest();
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }


        [HttpGet("/get-product-review-by-id")]
        public static async Task<IResult> GetByIdReview(int id, IMediator mediator, IMapper mapper)
        {
            var command = new GetDetailReviewRequest();
            command.ProductId = id;
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }

        [HttpGet("//get-product-cmt-start")]
        public static async Task<IResult> GetCmtStart(int id, IMediator mediator, IMapper mapper)
        {
            var command = new GetProductReviewSummaryRequest();
            command.ProductId = id;
            var result = await mediator.Send(command);
            return TypedResults.Ok(result);
        }
    }
}
