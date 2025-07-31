using chuyennganh.Application.Repositories.OrderRepo;
using chuyennganh.Domain.DTOs;
using chuyennganh.Domain.Enumerations;
using chuyennganh.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace chuyennganh.Api.Controllers
{
    [Route("api/payos")]
    [ApiController]
    public class PayosController : ControllerBase
    {
        private readonly PayosService _payosService;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<PayosController> _logger;

        public PayosController(
            PayosService payosService,
            IOrderRepository orderRepository,
            ILogger<PayosController> logger)
        {
            _payosService = payosService;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] PayOSRequestDto request)
        {
            try
            {
                var checkoutUrl = await _payosService.CreatePaymentUrlAsync(
                        request.OrderId,
                        request.Amount,
                        request.Description,
                        request.BuyerName,    // ✅ đúng
                        request.BuyerEmail,   // ✅ đúng
                        request.BuyerPhone,   // ✅ đúng
                        request.Items
                    );


                return Ok(new { checkoutUrl });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Lỗi khi tạo thanh toán PayOS");
                return BadRequest(new { error = ex.Message });
            }
        }


        /// ✅ IPN: PayOS gửi callback khi trạng thái đơn hàng thay đổi
        //[HttpPost("ipn")]
        //public async Task<IActionResult> IpnCallback()
        //{
        //    try
        //    {
        //        using var reader = new StreamReader(Request.Body);
        //        var body = await reader.ReadToEndAsync();

        //        var jsonDoc = JsonDocument.Parse(body);
        //        var root = jsonDoc.RootElement;

        //        var orderId = root.GetProperty("orderCode").GetInt32(); // ✅ Lấy đúng kiểu
        //        var status = root.GetProperty("status").GetString();

        //        _logger.LogInformation("📩 IPN PayOS nhận được: {0}", body);

        //        OrderStatus? newStatus = status switch
        //        {
        //            "PAID" => OrderStatus.Accepted,
        //            "CANCEL" => OrderStatus.Canceled,
        //            _ => null
        //        };

        //        if (newStatus.HasValue)
        //        {
        //            var success = await _orderRepository.UpdateOrderStatusAsync(orderId, newStatus.Value);
        //            if (success)
        //            {
        //                _logger.LogInformation($"✔️ Đã cập nhật đơn hàng #{orderId} sang trạng thái {newStatus}");
        //                return Ok();
        //            }
        //            else
        //            {
        //                _logger.LogWarning($"⚠️ Không tìm thấy đơn hàng có ID = {orderId}");
        //            }
        //        }

        //        return BadRequest("❌ Không thể xử lý: trạng thái không xác định.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "❌ Lỗi khi xử lý IPN từ PayOS");
        //        return StatusCode(500, "Lỗi hệ thống khi xử lý IPN.");
        //    }
        //}
        [HttpPost("ipn")]
        public async Task<IActionResult> IpnCallback()
        {
            try
            {
                using var reader = new StreamReader(Request.Body);
                var body = await reader.ReadToEndAsync();

                _logger.LogInformation("📩 IPN PayOS nhận được: {0}", body);

                var jsonDoc = JsonDocument.Parse(body);
                var root = jsonDoc.RootElement;

                var orderCodeStr = root.GetProperty("orderCode").GetString();

                if (!long.TryParse(orderCodeStr, out long orderCode))
                {
                    _logger.LogWarning("❌ orderCode không hợp lệ: {0}", orderCodeStr);
                    return BadRequest("orderCode không hợp lệ.");
                }

                int orderId = (int)(orderCode / 1000);


                var status = root.GetProperty("status").GetString()?.ToUpper();

                if (string.IsNullOrEmpty(status))
                {
                    _logger.LogWarning("❌ Thiếu trường 'status' trong IPN.");
                    return BadRequest("Thiếu trạng thái.");
                }

                // Ánh xạ trạng thái từ PayOS sang OrderStatus hệ thống
                OrderStatus? newStatus = status switch
                {
                    "PAID" => OrderStatus.Accepted,
                    "CANCEL" => OrderStatus.Canceled,
                    _ => null
                };

                if (!newStatus.HasValue)
                {
                    _logger.LogWarning("❌ Trạng thái không xác định từ PayOS: {0}", status);
                    return BadRequest("Trạng thái không hợp lệ.");
                }

                // Cập nhật đơn hàng
                var success = await _orderRepository.UpdateOrderStatusAsync(orderId, newStatus.Value);
                if (!success)
                {
                    _logger.LogWarning($"⚠️ Không tìm thấy hoặc không cập nhật được đơn hàng có ID = {orderId}");
                    return NotFound($"Không tìm thấy đơn hàng có ID = {orderId}");
                }

                _logger.LogInformation($"✔️ Đã cập nhật đơn hàng #{orderId} sang trạng thái {newStatus}");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Lỗi hệ thống khi xử lý IPN từ PayOS");
                return StatusCode(500, "Lỗi hệ thống.");
            }
        }

    }
}
