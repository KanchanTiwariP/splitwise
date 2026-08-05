using SplitWise.Domain.Common;
using SplitWise.Domain.Enums;

namespace SplitWise.Domain.Entities;

public class Expense :BaseEntity
{
    public string Description { get; set; } = string.Empty;

    public decimal TotalAmount { get; set; }

    public DateTime ExpenseDate { get; set; }

    public SplitType SplitType { get; set; }

    public int PaidBy { get; set; }

    public int GroupId { get; set; }

    public User? Payer { get; set; } = null!;

    public Group? Group { get; set; } = null!;
    
    public ICollection<ExpenseSplit> ExpenseSplits { get; set; } = new List<ExpenseSplit>();
}