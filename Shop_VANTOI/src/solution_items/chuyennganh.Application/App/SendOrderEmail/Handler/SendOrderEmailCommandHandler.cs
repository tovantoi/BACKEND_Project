using chuyennganh.Application.App.SendOrderEmail.Command;
using chuyennganh.Application.Repositories;
using chuyennganh.Application.Response;
using MediatR;
using System.Text;

namespace chuyennganh.Application.App.SendOrderEmail.Handler
{
    public class SendOrderEmailCommandHandler : IRequestHandler<SendOrderEmailCommand, ServiceResponse>
    {
        private readonly IEmailService _emailService;

        public SendOrderEmailCommandHandler(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<ServiceResponse> Handle(SendOrderEmailCommand request, CancellationToken cancellationToken)
        {
            var subject = $"🧾 Xác nhận đơn hàng #{request.OrderCode} - Shop Văn Tới";

            var sb = new StringBuilder();

            sb.AppendLine(@"
        <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; color: #333; border: 1px solid #eee; border-radius: 10px; overflow: hidden;'>
            <div style='padding: 20px; border-bottom: 3px solid #28a745; text-align: center; background-color: #f9f9f9;'>
                <img src='https://drive.google.com/uc?export=view&id=1aOutIk00DMVmKlkNDRD8ID83L4I_d9KR' 
                     alt='VANTOI Logo' 
                     style='width: 120px; border-radius: 12px; box-shadow: 0 0 10px rgba(0,0,0,0.1);' />
                <h2 style='color: #28a745; margin: 10px 0 0; font-weight: bold;'>Cảm ơn bạn đã đặt hàng tại VANTOI!</h2>
            </div>

            <div style='padding: 20px;'>
        ");

            sb.AppendLine($"<p>Xin chào <strong>{request.CustomerName}</strong>,</p>");
            sb.AppendLine($"<p>Chúng tôi đã nhận được đơn hàng của bạn với mã đơn <strong style='color:#28a745;'>{request.OrderCode}</strong>.</p>");
            sb.AppendLine("<p><strong>Chi tiết đơn hàng:</strong></p>");

            sb.AppendLine("<table style='width:100%; border-collapse: collapse;'>");
            sb.AppendLine("<thead><tr style='background-color:#f0f0f0;'>");
            sb.AppendLine("<th style='padding:8px; border:1px solid #ddd;'>Sản phẩm</th>");
            sb.AppendLine("<th style='padding:8px; border:1px solid #ddd;'>Đơn giá</th>");
            sb.AppendLine("<th style='padding:8px; border:1px solid #ddd;'>Số lượng</th>");
            sb.AppendLine("<th style='padding:8px; border:1px solid #ddd;'>Thành tiền</th>");
            sb.AppendLine("</tr></thead><tbody>");

            foreach (var item in request.OrderItems)
            {
                sb.AppendLine("<tr>");
                decimal unitPrice = item.Quantity > 0 ? item.Price / item.Quantity : 0;

                sb.AppendLine($"<td style='padding:8px; border:1px solid #ddd;'>{item.ProductName}</td>");
                sb.AppendLine($"<td style='padding:8px; border:1px solid #ddd; text-align:right;'>{unitPrice:N0} VND</td>");
                sb.AppendLine($"<td style='padding:8px; border:1px solid #ddd; text-align:center;'>{item.Quantity}</td>");
                sb.AppendLine($"<td style='padding:8px; border:1px solid #ddd; text-align:right;'>{item.Price:N0} VND</td>");

                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody></table>");

            // 🟡 Hiển thị chi tiết giá tiền
            decimal productOnlyTotal = request.OrderItems.Sum(x => x.Price);

            sb.AppendLine("<br/>");
            sb.AppendLine($"<p><strong>Tổng tiền sản phẩm:</strong> {productOnlyTotal:N0} VND</p>");
            sb.AppendLine($"<p><strong>Phí vận chuyển:</strong> {request.ShippingFee:N0} VND</p>");
            sb.AppendLine($"<p><strong>Tổng tiền (chưa giảm):</strong> {request.TotalBeforeDiscount:N0} VND</p>");
            Console.WriteLine("COUPON: " + request.CouponCode);
            Console.WriteLine("DISCOUNT: " + request.DiscountAmount);
            Console.WriteLine("SHIPPING: " + request.ShippingFee);

            // 🎯 Hiển thị mã giảm giá nếu có
            if (!string.IsNullOrWhiteSpace(request.CouponCode))
            {
                var discountText = request.DiscountAmount > 0
                    ? $" (<strong>-{request.DiscountAmount:N0} VND</strong>)"
                    : "";

                sb.AppendLine($"<p style='color:green;'><strong>Mã giảm giá:</strong> {request.CouponCode}{discountText}</p>");
            }

            sb.AppendLine($"<p style='margin-top:10px; font-size:16px;'><strong>Tổng tiền cần thanh toán: </strong><span style='color:#e53935; font-weight:bold;'>{request.TotalPrice:N0} VND</span></p>");

            sb.AppendLine("<p>Chúng tôi sẽ sớm xử lý đơn hàng và giao đến bạn trong thời gian sớm nhất.</p>");
            sb.AppendLine("<p>Trân trọng,<br/>Đội ngũ <strong>VANTOI</strong> 💚</p>");
            sb.AppendLine("</div>");

            sb.AppendLine(@"
        <div style='background-color: #28a745; color: white; padding: 10px; text-align: center; font-size: 12px;'>
            © 2025 VANTOI - Cảm ơn bạn đã tin tưởng lựa chọn chúng tôi
        </div>
    </div>");

            await _emailService.SendEmailAsync(request.Email, subject, sb.ToString());
            return ServiceResponse.Success("Gửi đơn hàng thành công");
        }
    }

}
