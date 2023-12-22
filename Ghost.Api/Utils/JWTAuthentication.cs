using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Ghost.Api.Utils;
public static class JWTAuthentication
{
  private const string defaultKey = "CZGA23XVL+4xaHAVOVNrsaOrxDezMP2Qj0mWDI5x/S4nPfo1OBsXZpTkUZQiF+U9VV61lWYBSczx8wrzeE7ANg==";
  public static string BuildJWTToken(int userId, string username)
  {
    var key = Convert.FromBase64String(Environment.GetEnvironmentVariable("JWT_SECRET") ?? defaultKey);
    var tokenHandler = new JwtSecurityTokenHandler();

    var now = DateTime.UtcNow;
    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(new[]
        {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.PrimarySid, userId.ToString())
                }),
      Expires = now.AddMinutes(Convert.ToInt32(Environment.GetEnvironmentVariable("JWT_EXPIRY") ?? "60")),
      SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(key),
            SecurityAlgorithms.HmacSha256Signature
        )
    };

    var stoken = tokenHandler.CreateToken(tokenDescriptor);
    var token = tokenHandler.WriteToken(stoken);

    return token;
  }

  public static string BuildSymKey()
  {
    var hmac = new HMACSHA256();
    var key = Convert.ToBase64String(hmac.Key);
    return key;
  }

  public static void ConfigureJWTAuthentication(IServiceCollection services)
  {
    var key = Convert.FromBase64String(Environment.GetEnvironmentVariable("JWT_SECRET") ?? defaultKey);
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
          };
        });
  }
}