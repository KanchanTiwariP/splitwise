using Microsoft.AspNetCore.Mvc;
using SplitWise.Application.DTOs.Group;
using SplitWise.Application.Interfaces.Services;

namespace SplitWise.API.Controllers;

[Route("api/[controller]")]
public class GroupController : BaseController
{
    private readonly IGroupService _groupService;
    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    [HttpGet]
    public async Task<IActionResult> GetGroups()
    {
        var result = await _groupService.GetUserGroupsAsync(CurrentUserId);
        return Ok(result);
    }
    
     [HttpGet("{id:int}")]
     public async Task<IActionResult> GetGroup(int id)
     {
         var group = await _groupService
             .GetGroupByIdAsync(id, CurrentUserId);

         if (group == null)
             return NotFound();

         return Ok(group);
     }
     
     [HttpPost("{groupId:int}/members")]
     public async Task<IActionResult> AddMember(int groupId, AddGroupMemberRequest request)
     {
         await _groupService.AddMemberAsync(groupId, CurrentUserId, request);
         return Ok();
     }
     
    [HttpPost]
    public async Task<IActionResult> CreateGroup(CreateGroupRequest request)
    {
        var group = await _groupService.CreateGroupAsync(CurrentUserId, request);
        return Ok(group);
    }
    
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateGroup(int id, UpdateGroupRequest request)
    {
        var group = await _groupService.UpdateGroupAsync(
            id,
            CurrentUserId,
            request);

        if (group == null)
            return NotFound();

        return Ok(group);
    }
}