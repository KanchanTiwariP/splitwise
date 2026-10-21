using System.Security.Cryptography;
using System.Text;
using SplitWise.Application.Interfaces.Services;

namespace SplitWise.Infrastructure.Services;

public class RefreshTokenService :IRefreshTokenService
{
    public string GenerateToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }

    public string HashToken(string token)
    {
        var hash = SHA256.HashData(
            Encoding.UTF8.GetBytes(token));
        return Convert.ToHexString(hash);
    }
}