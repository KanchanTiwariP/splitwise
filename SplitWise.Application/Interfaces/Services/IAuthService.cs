using SplitWise.Application.DTOs.Auth;

namespace SplitWise.Application.Interfaces.Services;

public interface IAuthService
{
    Task<RegisterResponse> RegisterAsync(RegisterRequest request);
    
    Task<LoginResponse> LoginAsync(LoginRequest request);
}