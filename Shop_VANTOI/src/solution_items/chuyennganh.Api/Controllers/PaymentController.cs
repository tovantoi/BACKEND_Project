using AutoMapper;
using chuyennganh.Application.App.PaymentTransaction.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace chuyennganh.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        [HttpPost("/create-payment")]
        public static async Task<IResult> CreatePayment(
            [FromBody] CreatePaymentTransactionRequest request,
            IMediator mediator,
            IMapper mapper)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                // ✅ Trích xuất paymentUrl từ query
                var paymentUrl = result.Query?.GetType().GetProperty("paymentUrl")?.GetValue(result.Query)?.ToString();

                if (!string.IsNullOrEmpty(paymentUrl))
                {
                    return Results.Ok(new { paymentUrl });
                }

                // Nếu không có URL
                return Results.BadRequest(new { message = "Không thể tạo link thanh toán." });
            }

            return Results.BadRequest(new { message = result.Message ?? "Đã xảy ra lỗi." });
        }

    }
}
