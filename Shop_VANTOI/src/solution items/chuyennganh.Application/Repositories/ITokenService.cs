//using chuyennganh.Domain.Entities;
//using Microsoft.Extensions.Options;
//using System.Text;

//public interface ITokenService
//{
//    string GenerateToken(Customer user);
//}

//public class TokenService : ITokenService
//{
//    private readonly JwtSettings _jwtSettings;

//    public TokenService(IOptions<JwtSettings> jwtSettings)
//    {
//        _jwtSettings = jwtSettings.Value;
//    }

//    public string GenerateToken(Customer user)
//    {
//        if (user == null)
//            throw new ArgumentNullException(nameof(user), "User cannot be null");

//        if (string.IsNullOrEmpty(user.UserName)) // Kiểm tra Username
//            throw new ArgumentNullException(nameof(user.UserName), "Username cannot be null or empty");

//        var tokenString = user.UserName;
//        return Convert.ToBase64String(Encoding.UTF8.GetBytes(tokenString));
//    }
//}
