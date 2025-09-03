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


        //[HttpPost("ipn")]
        //public async Task<IActionResult> IpnCallback()
        //{
        //    try
        //    {
        //        using var reader = new StreamReader(Request.Body);
        //        var body = await reader.ReadToEndAsync();
        //        _logger.LogInformation("📩 IPN PayOS nhận được: {0}", body);
        //        var jsonDoc = JsonDocument.Parse(body);
        //        var root = jsonDoc.RootElement;

        //        // Truy cập vào thuộc tính "data" trước
        //        if (root.TryGetProperty("data", out var dataElement))
        //        {
        //            if (!dataElement.TryGetProperty("orderCode", out var orderCodeElement) ||
        //         !orderCodeElement.TryGetInt64(out long orderCode))
        //            {
        //                _logger.LogWarning("❌ Thiếu hoặc không hợp lệ 'orderCode' trong IPN.");
        //                return BadRequest("Thiếu hoặc không hợp lệ 'orderCode'.");
        //            }

        //            int orderId = (int)(orderCode / 1000);

        //            // Kiểm tra và lấy status
        //            if (!dataElement.TryGetProperty("status", out var statusElement) ||
        //                string.IsNullOrEmpty(statusElement.GetString()))
        //            {
        //                _logger.LogWarning("❌ Thiếu trường 'status' trong IPN.");
        //                return BadRequest("Thiếu trạng thái.");
        //            }

        //            var status = statusElement.GetString()?.ToUpper();

        //            // Ánh xạ trạng thái từ PayOS sang OrderStatus hệ thống
        //            OrderStatus? newStatus = status switch
        //            {
        //                "PAID" => OrderStatus.Accepted,
        //                "CANCEL" => OrderStatus.Canceled,
        //                _ => null
        //            };

        //            if (!newStatus.HasValue)
        //            {
        //                _logger.LogWarning("❌ Trạng thái không xác định từ PayOS: {0}", status);
        //                return BadRequest("Trạng thái không hợp lệ.");
        //            }

        //            // Cập nhật đơn hàng
        //            var success = await _orderRepository.UpdateOrderStatusAsync(orderId, newStatus.Value);
        //            if (!success)
        //            {
        //                _logger.LogWarning($"⚠️ Không tìm thấy hoặc không cập nhật được đơn hàng có ID = {orderId}");
        //                return NotFound($"Không tìm thấy đơn hàng có ID = {orderId}");
        //            }

        //            _logger.LogInformation($"✔️ Đã cập nhật đơn hàng #{orderId} sang trạng thái {newStatus}");
        //            return Ok();
        //        }

        //        return BadRequest("Thiếu thuộc tính 'data' trong IPN.");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "❌ Lỗi hệ thống khi xử lý IPN từ PayOS");
        //        return StatusCode(500, "Lỗi hệ thống.");
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

                // Lấy object "data"
                if (!root.TryGetProperty("data", out var data))
                {
                    _logger.LogWarning("❌ Không có trường 'data' trong IPN.");
                    return BadRequest("Thiếu trường data.");
                }

                // Lấy orderCode từ data
                var orderCodeStr = data.GetProperty("orderCode").GetRawText(); // có thể là number
                if (!long.TryParse(orderCodeStr, out long orderCode))
                {
                    _logger.LogWarning("❌ orderCode không hợp lệ: {0}", orderCodeStr);
                    return BadRequest("orderCode không hợp lệ.");
                }

                int orderId = (int)orderCode;

                // Lấy code (trạng thái thanh toán) từ data
                var payosCode = data.GetProperty("code").GetString();

                if (string.IsNullOrEmpty(payosCode))
                {
                    _logger.LogWarning("❌ Thiếu trường 'code' trong data.");
                    return BadRequest("Thiếu trạng thái.");
                }

                // Ánh xạ code từ PayOS sang OrderStatus hệ thống
                OrderStatus? newStatus = payosCode switch
                {
                    "00" => OrderStatus.Accepted,  // Thành công
                    "07" => OrderStatus.Canceled,  // Thất bại/hủy
                    _ => null
                };

                if (!newStatus.HasValue)
                {
                    _logger.LogWarning("❌ Trạng thái không xác định từ PayOS: {0}", payosCode);
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
                return Ok(new { message = "Cập nhật đơn hàng thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Lỗi hệ thống khi xử lý IPN từ PayOS");
                return StatusCode(500, "Lỗi hệ thống.");
            }
        }
    }
}
