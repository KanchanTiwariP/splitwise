using SplitWise.Application.DTOs.Auth;
using SplitWise.Application.Exceptions;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Application.Interfaces.Services;
using SplitWise.Domain.Entities;

namespace SplitWise.Infrastructure.Services;

public class AuthService :IAuthService
{
    private readonly IUserRepository  _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    
    public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator,  IRefreshTokenService refreshTokenService, IRefreshTokenRepository refreshTokenRepository)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _refreshTokenService = refreshTokenService;
        _refreshTokenRepository = refreshTokenRepository;
    }
    
    public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
    {
        if (await _userRepository.EmailExistsAsync(request.Email))
        {
            throw new ValidationException("Email already exists.");
        } 
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };
        
        await _userRepository.AddUserAsync(user);
        await _userRepository.SaveChangesAsync();

        return new RegisterResponse()
        {   
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        };
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null)
            throw new UnauthorizedException("Invalid email or password.");
        
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Invalid email or password.");
        
        var accessToken = _jwtTokenGenerator.GenerateToken(user);
        var refreshToken = _refreshTokenService.GenerateToken();
        var refreshTokenHash = _refreshTokenService.HashToken(refreshToken);
        
        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = refreshTokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        await _refreshTokenRepository.AddAsync(refreshTokenEntity);
        await _refreshTokenRepository.SaveChangesAsync();
        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            FirstName = user.FirstName,
            Email = user.Email,
        };
    }

    public async Task<LoginResponse> RefreshAsync(string refreshToken)
    { var tokenHash = _refreshTokenService.HashToken(refreshToken);

        var storedToken =
            await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

        if (storedToken == null)
            throw new UnauthorizedException("Invalid refresh token.");

        if (!storedToken.IsActive)
            throw new UnauthorizedException("Refresh token is expired or revoked.");

        var user = await _userRepository.GetUserByIdAsync(storedToken.UserId);

        if (user == null || !user.IsActive)
            throw new UnauthorizedException("Invalid refresh token.");

        // Generate new access token
        var accessToken = _jwtTokenGenerator.GenerateToken(user);

        // Generate new refresh token
        var newRefreshToken = _refreshTokenService.GenerateToken();
        var newRefreshTokenHash =
            _refreshTokenService.HashToken(newRefreshToken);

        var newRefreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = newRefreshTokenHash,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
        
        await _refreshTokenRepository.RotateTokenAsync(
            storedToken,
            newRefreshTokenEntity);
        
        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken,
            FirstName = user.FirstName,
            Email = user.Email
        };
    }
    
    public async Task LogoutAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            throw new ValidationException("Refresh token is required.");

        var tokenHash = _refreshTokenService.HashToken(refreshToken);

        var storedToken =
            await _refreshTokenRepository.GetByTokenHashAsync(tokenHash);

        // Logout is idempotent: an unknown or already-revoked
        // token doesn't need another database update.
        if (storedToken == null || storedToken.IsRevoked)
            return;

        storedToken.RevokedAt = DateTime.UtcNow;

        await _refreshTokenRepository.SaveChangesAsync();
    }
    public List<int> solve(List<int> A, List<List<int>> B)
    {
        List<int> ans = new List<int>();
        List<bool> isDivisible = new List<bool>();
        for (int i = 0; i < A.Count; i++)
        {
            if(A[i] % 7 == 0)
                isDivisible.Add(true);
            else
            {
                isDivisible.Add(false);
            }
        }
        for (int i = 0; i < B.Count; i++)
        {
            int xrange = B[i][0];
            int yrange = B[i][1];
            int count = 0;
            for (int j = xrange; j < yrange; j++)
            {
                if (isDivisible[j])
                {
                    count++;
                }
            }
            ans.Add(count);
        }

        return ans;
    }
}