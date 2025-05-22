using chuyennganh.Application.ChatBot;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

[ApiController]
[Route("api/chatbot")]
public class ChatbotController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;

    public ChatbotController(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> Chat([FromBody] ChatRequest request)
    {
        var apiKey = _config["OpenAI:ApiKey"];
        Console.WriteLine($"API Key được load: {apiKey}");

        if (string.IsNullOrEmpty(apiKey))
        {
            return BadRequest(new { message = "API Key is missing!" });
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        // Log headers để kiểm tra xem có Authorization không
        Console.WriteLine($"Headers: {_httpClient.DefaultRequestHeaders}");

        var requestBody = new
        {
            model = "gpt-4o mini",
            messages = new[] { new { role = "user", content = request.Message } },
            max_tokens = 100
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("c", jsonContent);
        var result = await response.Content.ReadAsStringAsync();

        // Log kết quả từ OpenAI
        Console.WriteLine($"OpenAI Response: {result}");

        return Ok(JsonConvert.DeserializeObject<dynamic>(result));
    }

}


