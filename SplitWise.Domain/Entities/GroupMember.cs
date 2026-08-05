namespace SplitWise.Domain.Entities;

public class GroupMember
{
    public int GroupId { get; set; }

    public int UserId { get; set; }

    public DateTime JoinedAt { get; set; }

    public DateTime? LeftAt { get; set; } = null;
    
    public Group Group { get; set; } = null!;

    public User User { get; set; } = null!;
    
}