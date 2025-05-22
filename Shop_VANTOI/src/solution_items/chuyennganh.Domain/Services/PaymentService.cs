using System.Net.Http.Headers;
using chuyennganh.Domain.Abstractions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Globalization;

public class PaymentService : IPaymentService
{
    private readonly IConfiguration _configuration;

    public PaymentService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> CreatePayPalPayment(decimal amount, string orderId, string returnUrl)
    {
        var clientId = _configuration["PayPal:ClientId"];
        var secret = _configuration["PayPal:Secret"];
        var basicAuth = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secret}"));

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuth);

        // 1. Lấy access_token (nếu chưa cache)
        var tokenResponse = await client.PostAsync("https://api-m.sandbox.paypal.com/v1/oauth2/token",
            new FormUrlEncodedContent(new Dictionary<string, string> {
                { "grant_type", "client_credentials" }
            }));
        var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
        var accessToken = JObject.Parse(tokenJson)["access_token"]?.ToString();
        var usdAmount = amount / 24000m; 

        if (string.IsNullOrEmpty(accessToken)) throw new Exception("Không lấy được access token từ PayPal.");

        // 2. Tạo đơn hàng
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var payload = new
        {
            intent = "CAPTURE",
            purchase_units = new[] {
                new {
                    amount = new {
                        currency_code = "USD",
                        value = usdAmount.ToString("F2", CultureInfo.InvariantCulture)
                    },
                    custom_id = orderId
                }
            },
            application_context = new
            {
                return_url = returnUrl,
                cancel_url = returnUrl
            }
        };

        var content = new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json");
        var response = await client.PostAsync("https://api-m.sandbox.paypal.com/v2/checkout/orders", content);
        var responseJson = await response.Content.ReadAsStringAsync();

        var json = JObject.Parse(responseJson);
        var approveUrl = json["links"]?.FirstOrDefault(x => x["rel"]?.ToString() == "approve")?["href"]?.ToString();
        Console.WriteLine("PayPal Approve URL: " + approveUrl);
        Console.WriteLine("PayPal raw response:");
        Console.WriteLine(responseJson);
        return approveUrl ?? throw new Exception("Không tạo được URL thanh toán.");
    }
}
