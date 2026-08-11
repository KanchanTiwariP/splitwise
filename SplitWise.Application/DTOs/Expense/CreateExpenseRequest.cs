using SplitWise.Domain.Enums;

namespace SplitWise.Application.DTOs.Expense;

public class CreateExpenseRequest
{
    public string Description { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public SplitType SplitType { get; set; }
    public int PaidBy { get; set; }
    public List<int> UserIds { get; set; } = new();
}