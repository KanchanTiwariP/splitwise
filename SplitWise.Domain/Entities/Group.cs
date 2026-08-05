using SplitWise.Domain.Common;
namespace SplitWise.Domain.Entities;

public class Group : BaseEntity
{
    public string Name {get; set; } = string.Empty;
    
    public int CreatedBy { get; set; }
    
    public User? Creator { get; set; } = null!;
    
    public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();

    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();

    public ICollection<Settlement> Settlements { get; set; } = new List<Settlement>();
}