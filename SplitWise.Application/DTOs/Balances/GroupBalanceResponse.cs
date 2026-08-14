namespace SplitWise.Application.DTOs.Balances;

public class GroupBalanceResponse
{
    public int UserId { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalShare { get; set; }
    public decimal Balance { get; set; }
}