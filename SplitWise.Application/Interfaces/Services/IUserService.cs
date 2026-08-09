using SplitWise.Application.DTOs;

namespace SplitWise.Application.Interfaces.Services;

public interface IUserService
{
     Task<UserResponse?>  GetMeAsync(int userId);
     Task<UserResponse?> UpdateMeAsync(int userId, UpdateUserRequest request);
}