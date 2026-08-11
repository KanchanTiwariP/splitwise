using SplitWise.Application.DTOs.Group;

namespace SplitWise.Application.Interfaces.Services;

public interface IGroupService
{ 
    Task<List<GroupResponse>> GetUserGroupsAsync(int userId);
    Task<GroupResponse?> GetGroupByIdAsync(int groupId, int userId);
    Task<GroupResponse> CreateGroupAsync(int userId, CreateGroupRequest request);
    Task AddMemberAsync( int groupId,int currentUserId,AddGroupMemberRequest request);
    Task<GroupResponse?> UpdateGroupAsync(int groupId, int currentUserId, UpdateGroupRequest request);
}