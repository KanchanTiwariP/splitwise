using Microsoft.EntityFrameworkCore;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Domain.Entities;
using SplitWise.Infrastructure.Persistence;

namespace SplitWise.Infrastructure.Repositories;

public class GroupMemberRepository : IGroupMemberRepository
{
    private readonly AppDbContext _context;

    public GroupMemberRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddMemberAsync(GroupMember groupMember)
    {
        await _context.GroupMembers.AddAsync(groupMember);
    }

    public async Task<bool> IsActiveMemberAsync(int groupId, int userId)
    {
        return await _context.GroupMembers
            .AnyAsync(x =>
                x.GroupId == groupId &&
                x.UserId == userId &&
                x.LeftAt == null);
    }

    public async Task<bool> AreActiveMembersAsync(int groupId, IEnumerable<int> userIds)
    {
        var activeMemberCount = await _context.GroupMembers
            .CountAsync(x =>
                x.GroupId == groupId &&
                x.LeftAt == null &&
                userIds.Contains(x.UserId));

        return activeMemberCount == userIds.Distinct().Count();
    }
}