using AutoMapper;
using chuyennganh.Application.App.ProdcutReview.Command;
using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Application.Repositories.ProductRepo;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace chuyennganh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewRepository productReviewRepository;
        private readonly IOrderRepository orderRepository;

        public ProductReviewController(
            IProductReviewRepository productReviewRepository,
            IOrderRepository orderRepository)
        {
            this.productReviewRepository = productReviewRepository;
            this.orderRepository = orderRepository;
        }

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

        [HttpGet("check-product-review")]
        public async Task<IActionResult> CheckReview(int userId, int productId)
        {
            try
            {
                var hasPurchased = await orderRepository.HasUserPurchasedProductAsync(userId, productId);
                var hasReviewed = await productReviewRepository.HasUserReviewedProductAsync(userId, productId);

                return Ok(new { canReview = hasPurchased && !hasReviewed });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi kiểm tra quyền đánh giá.", error = ex.Message });
            }
        }
    }
}
