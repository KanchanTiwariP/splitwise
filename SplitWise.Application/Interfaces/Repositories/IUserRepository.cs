using SplitWise.Application.DTOs;
using SplitWise.Domain.Entities;

namespace SplitWise.Application.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(int id);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task AddUserAsync(User user);
    
    Task<User?> UpdateUserAsync(int userId, UpdateUserRequest request);
    Task<int> SaveChangesAsync();
}