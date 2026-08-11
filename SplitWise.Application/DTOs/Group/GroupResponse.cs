using SplitWise.Domain.Entities;

namespace SplitWise.Application.DTOs.Group;

public class GroupResponse
{
    public int Id { get; set; }
    public string Name {get; set; } = string.Empty;
    public int CreatedBy { get; set; }
    public List<GroupMemberResponse> Members { get; set; } = new();
}