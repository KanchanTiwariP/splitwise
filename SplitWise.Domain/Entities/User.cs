using SplitWise.Domain.Common;

namespace SplitWise.Domain.Entities;

public class User : BaseEntity
{
    public string FirstName {get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public ICollection<GroupMember> GroupMembers { get; set; }= new List<GroupMember>();
    public ICollection<Group> CreatedGroups { get; set; } = new List<Group>();
    public ICollection<Expense> PaidExpenses { get; set; } = new List<Expense>();
    public ICollection<ExpenseSplit> ExpenseSplits { get; set; } = new List<ExpenseSplit>();

    public ICollection<Settlement> PaymentsMade { get; set; } = new List<Settlement>();

    public ICollection<Settlement> PaymentsReceived { get; set; } = new List<Settlement>();
} 