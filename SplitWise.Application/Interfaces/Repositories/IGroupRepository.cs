using SplitWise.Domain.Entities;

namespace SplitWise.Application.Interfaces.Repositories;

public interface IGroupRepository
{
    Task<List<Group>> GetUserGroupsAsync(int id);
    Task<Group?> GetGroupByIdAsync(int groupId, int userId);
    Task<Group?> GetByIdAsync(int groupId);
    Task<Group> AddAsync(Group group);
    Task SaveChangesAsync();
}