using SplitWise.Domain.Entities;

namespace SplitWise.Application.Interfaces.Services;

public interface IGroupAccessService
{
    Task<Group> GetGroupAsync(int groupId, int currentUserId);
}