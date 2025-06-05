using chuyennganh.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace chuyennganh.Api.Controllers
{
    [Route("api/vnpay")]
    public class VnPayController : Controller
    {
        private readonly VnPayService _vnPayService;

        public VnPayController(VnPayService vnPayService)
        {
            _vnPayService = vnPayService;
        }

        [HttpGet("create")]
        public IActionResult CreatePayment(decimal amount, string orderId, string orderDescription)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            var paymentUrl = _vnPayService.CreatePaymentUrl(amount, orderId, orderDescription, ipAddress);
            return Redirect(paymentUrl);
        }

        [HttpGet("return")]
        public IActionResult PaymentReturn()
        {
            var isValid = _vnPayService.ValidateSignature(Request.Query);
            if (isValid)
            {
                // Xử lý đơn hàng thành công
                return Content("Thanh toán thành công");
            }
            else
            {
                // Xử lý đơn hàng thất bại
                return Content("Thanh toán thất bại");
            }
        }

        [HttpGet("ipn")]
        public IActionResult PaymentNotification()
        {
            var isValid = _vnPayService.ValidateSignature(Request.Query);
            if (isValid)
            {
                // Cập nhật trạng thái đơn hàng trong hệ thống
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }

}
