using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace chuyennganh.Domain.Services
{
    public class SocialLoginService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public SocialLoginService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<(bool success, string email, string name, string givenName, string familyName, string picture, string subject)> VerifyGoogleTokenAsync(string token)
        {
            var client = _httpClientFactory.CreateClient();
            var googleClientId = _configuration["Authentication:Google:ClientId"];

            var response = await client.GetStringAsync($"https://oauth2.googleapis.com/tokeninfo?id_token={token}");
            var json = JsonDocument.Parse(response).RootElement;

            var audience = json.GetProperty("aud").GetString();
            if (audience != googleClientId)
                return (false, null, null, null, null, null, null);

            var email = json.GetProperty("email").GetString();
            var name = json.GetProperty("name").GetString();
            var givenName = json.GetProperty("given_name").GetString();
            var familyName = json.GetProperty("family_name").GetString();
            var picture = json.GetProperty("picture").GetString();
            var subject = json.GetProperty("sub").GetString();

            return (true, email, name, givenName, familyName, picture, subject);
        }   
    }
}
