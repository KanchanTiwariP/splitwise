using Microsoft.EntityFrameworkCore;
using SplitWise.Application.DTOs;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Domain.Entities;
using SplitWise.Infrastructure.Persistence;

namespace SplitWise.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users
            .AnyAsync(x => x.Email == email);
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public Task<User?> UpdateUserAsync(int userId, UpdateUserRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<int> SaveChangesAsync()
    {
       return await _context.SaveChangesAsync();
    }
}