namespace SplitWise.Domain.Entities;

public class ExpenseSplit
{
    public int ExpenseId { get; set; }

    public int UserId { get; set; }

    public decimal ShareAmount { get; set; }

    public Expense Expense { get; set; } = null!;

    public User User { get; set; } = null!;
}