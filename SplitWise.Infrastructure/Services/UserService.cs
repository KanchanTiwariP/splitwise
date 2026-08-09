using SplitWise.Application.DTOs;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Application.Interfaces.Services;

namespace SplitWise.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IUserRepository  _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public async Task<UserResponse?> GetMeAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
            return null;

        return new UserResponse
        {
           Id = user.Id,
           FirstName = user.FirstName,
           LastName = user.LastName,
           Email  = user.Email
        };
    }

    public async Task<UserResponse?> UpdateMeAsync(int userId, UpdateUserRequest request)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null)
            return null;
        
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        await _userRepository.SaveChangesAsync();

        return new UserResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }
}