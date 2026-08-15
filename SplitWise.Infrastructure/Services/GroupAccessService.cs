using SplitWise.Application.Exceptions;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Application.Interfaces.Services;
using SplitWise.Domain.Entities;

namespace SplitWise.Infrastructure.Services;

public class GroupAccessService : IGroupAccessService
{
    private readonly IGroupRepository _groupRepository;

    public GroupAccessService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<Group> GetGroupAsync(int groupId, int currentUserId)
    {
        var group = await _groupRepository
            .GetGroupByIdAsync(groupId, currentUserId);

        if (group == null)
            throw new NotFoundException("Group not found or you are not a member of this group.");
        return group;
    }
}