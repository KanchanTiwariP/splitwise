namespace SplitWise.Application.DTOs.Expense;

public class ExpenseResponse
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public DateTime ExpenseDate { get; set; }
    public int PaidBy { get; set; }
    public int GroupId { get; set; }

    public List<ExpenseSplitResponse> Splits { get; set; } = new();
}