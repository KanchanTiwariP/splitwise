namespace SplitWise.Application.DTOs.Balances;

public class SettlementSuggestionResponse
{
    public int FromUserId { get; set; }
    public int ToUserId { get; set; }
    public decimal Amount { get; set; }
}