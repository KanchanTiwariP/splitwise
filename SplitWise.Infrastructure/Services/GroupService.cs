using SplitWise.Application.DTOs.Group;
using SplitWise.Application.Exceptions;
using SplitWise.Application.Interfaces.Repositories;
using SplitWise.Application.Interfaces.Services;
using SplitWise.Domain.Entities;

namespace SplitWise.Infrastructure.Services;

public class GroupService :IGroupService
{
    private readonly IGroupRepository _groupRepository;
    private readonly  IUserRepository _userRepository;
    private readonly IGroupMemberRepository _groupMemberRepository;
    private readonly IGroupAccessService _groupAccessService;
    
    public GroupService(IGroupRepository groupRepository,  IGroupMemberRepository groupMemberRepository, IUserRepository userRepository, IGroupAccessService groupAccessService)
    {
        _groupRepository = groupRepository;
        _groupMemberRepository = groupMemberRepository;
        _userRepository = userRepository;
        _groupAccessService = groupAccessService;
    }
    public async Task<List<GroupResponse>> GetUserGroupsAsync(int userId)
    {
        var groups = await _groupRepository.GetUserGroupsAsync(userId);

        return groups.Select(group => new GroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            CreatedBy = group.CreatedBy
        }).ToList();
    }

    public async Task<GroupResponse?> GetGroupByIdAsync(int groupId,int userId)
    {
        var group = await _groupAccessService
            .GetGroupAsync(groupId, userId);

        return new GroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            CreatedBy = group.CreatedBy,
            Members = group.Members
                .Where(m => m.LeftAt == null)
                .Select(m => new GroupMemberResponse
                {
                    UserId = m.UserId,
                    FirstName = m.User.FirstName,
                    LastName = m.User.LastName,
                    Email = m.User.Email,
                    JoinedAt = m.JoinedAt
                })
                .ToList()
        };
    }

    public async Task<GroupResponse> CreateGroupAsync(int userId, CreateGroupRequest request)
    {
        var group = new Group
        {
            Name = request.Name,
            CreatedBy = userId
        };

        group.Members.Add(new GroupMember
        {
            UserId = userId,
            JoinedAt = DateTime.UtcNow
        });

        await _groupRepository.AddAsync(group);
        await _groupRepository.SaveChangesAsync();
        return new GroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            CreatedBy = group.CreatedBy,
        };
    }

    public async Task AddMemberAsync(
        int groupId,
        int currentUserId,
        AddGroupMemberRequest request)
    {
         await _groupAccessService
            .GetGroupAsync(groupId, currentUserId);

        var user = await _userRepository
            .GetUserByIdAsync(request.UserId);

        if (user == null)
            throw new NotFoundException("User not found.");

        var alreadyMember = await _groupMemberRepository
            .IsActiveMemberAsync(groupId, request.UserId);

        if (alreadyMember)
            throw new ValidationException("User is already a member of this group.");

        var member = new GroupMember
        {
            GroupId = groupId,
            UserId = request.UserId,
            JoinedAt = DateTime.UtcNow
        };

        await _groupMemberRepository.AddMemberAsync(member);
        await _groupRepository.SaveChangesAsync();
    }

    public async Task<GroupResponse?> UpdateGroupAsync(int groupId, int currentUserId, UpdateGroupRequest request)
    {
        var group = await _groupAccessService
                                .GetGroupAsync(groupId, currentUserId);

        if (group.CreatedBy != currentUserId)
        {
            throw new ForbiddenException("Only the group creator can update the group.");
        }

        group.Name = request.Name;
        group.UpdatedOn = DateTime.UtcNow;
        await _groupRepository.SaveChangesAsync();
        return new GroupResponse
        {
            Id = group.Id,
            Name = group.Name,
            CreatedBy = group.CreatedBy,
        };
    }
}