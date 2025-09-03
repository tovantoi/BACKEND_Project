using chuyennganh.Domain.DTOs;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace chuyennganh.Domain.Services
{
    public class PayosService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public PayosService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<string> CreatePaymentUrlAsync(
            int orderId,
            decimal amount,
            string description,
            string buyerName,
            string buyerEmail,
            string buyerPhone,
            List<OrderItemDTOPayos> orderItems)
        {
            var clientId = _configuration["PayOS:ClientId"];
            var apiKey = _configuration["PayOS:ApiKey"];
            var returnUrl = _configuration["PayOS:ReturnUrl"];
            var cancelUrl = _configuration["PayOS:CancelUrl"];
            var webhookUrl = _configuration["PayOS:WebhookUrl"];
            var checksumKey = _configuration["PayOS:ChecksumKey"];

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(checksumKey))
                throw new Exception("ClientId, ApiKey hoặc ChecksumKey bị thiếu.");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-client-id", clientId);
            _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);


            var items = orderItems
                 .Where(x => !string.IsNullOrWhiteSpace(x.ProductName) && x.Quantity.HasValue && x.Price.HasValue)
                 .Select(x => new
                 {
                     name = x.ProductName!,
                     quantity = x.Quantity.Value,
                     price = x.Price.Value
                 })
                 .ToList();

            Console.WriteLine("🧾 Danh sách sản phẩm nhận được:");
            foreach (var item in items)
            {
                Console.WriteLine($"  - Name: {item.name}, Price: {item.price}, Quantity: {item.quantity}");
            }


            int totalFromItems = items.Sum(x => x.quantity * x.price);
            int amountInt = (int)Math.Round(amount);

            if (totalFromItems != amountInt)
            {
                throw new Exception($"Tổng tiền từ sản phẩm ({totalFromItems}) không khớp với số tiền thanh toán ({amountInt}).");
            }

            // HOẶC nếu muốn tránh trùng, kết hợp đơn giản:
            var orderCode = orderId * 1000 + DateTime.Now.Second;

            // Generate signature theo định dạng tài liệu PayOS
            var dataToSign = $"amount={amountInt}&cancelUrl={cancelUrl}&description={description}&orderCode={orderCode}&returnUrl={returnUrl}";
            var signature = GenerateSignature(dataToSign, checksumKey);

            var payload = new
            {
                orderCode,
                amount = amountInt,
                description,
                returnUrl,
                cancelUrl,
                //webhookUrl,
                buyerName,
                buyerEmail,
                buyerPhone,
                items,
                signature
            };

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api-merchant.payos.vn/v2/payment-requests", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Phản hồi từ PayOS: " + responseContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"PayOS lỗi HTTP {(int)response.StatusCode}: {responseContent}");
            }

            using var document = JsonDocument.Parse(responseContent);
            if (document.RootElement.TryGetProperty("data", out var data) &&
                data.ValueKind == JsonValueKind.Object &&
                data.TryGetProperty("checkoutUrl", out var urlElement))
            {
                return urlElement.GetString()!;
            }

            throw new Exception("Không tìm thấy `checkoutUrl` trong phản hồi từ PayOS.");
        }

        private string GenerateSignature(string data, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var dataBytes = Encoding.UTF8.GetBytes(data);
            using var hmac = new HMACSHA256(keyBytes);
            var hashBytes = hmac.ComputeHash(dataBytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
