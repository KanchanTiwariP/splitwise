using SplitWise.Domain.Entities;

namespace SplitWise.Application.Interfaces.Repositories;

public interface IGroupMemberRepository
{
    Task AddMemberAsync(GroupMember groupMember);
    Task<bool> IsActiveMemberAsync(int groupId, int userId);
    Task<bool> AreActiveMembersAsync(int groupId, IEnumerable<int> userIds);
}