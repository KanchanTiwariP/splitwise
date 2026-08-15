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
    
    public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
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
            throw new ValidationException("Invalid email or password.");
        
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new ValidationException("Invalid email or password.");
        
        var token = _jwtTokenGenerator.GenerateToken(user);
        return new LoginResponse
        {
            Token = token,
            FirstName = user.FirstName,
            Email = user.Email,
        };
    }
}