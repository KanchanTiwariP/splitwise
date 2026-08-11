using Microsoft.EntityFrameworkCore;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Domain.Entities;
using SplitWise.Infrastructure.Persistence;

namespace SplitWise.Infrastructure.Repositories;

public class GroupRepository : IGroupRepository
{  
    private readonly AppDbContext _context;
    public GroupRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Group>> GetUserGroupsAsync(int userId)
    {
        return await _context.Groups
            .Where(g => g.Members.Any(m =>
                m.UserId == userId &&
                m.LeftAt == null))
            .ToListAsync();
    }

    public async Task<Group?> GetGroupByIdAsync(int groupId, int userId)
    {
        return await _context.Groups
            .Include(g => g.Members)
            .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(g =>
                g.Id == groupId &&
                g.Members.Any(m =>
                    m.UserId == userId &&
                    m.LeftAt == null));
    }
    
    public async Task<Group?> GetByIdAsync(int groupId)
    {
        return await _context.Groups
            .FirstOrDefaultAsync(g => g.Id == groupId);
    }
    public async Task<Group> AddAsync(Group group)
    {
        await _context.Groups.AddAsync(group);
        return group;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}