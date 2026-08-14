namespace SplitWise.Application.DTOs.Settlements;

public class CreateSettlementRequest
{
    public int PayerId { get; set; }

    public int ReceiverId { get; set; }

    public decimal Amount { get; set; }

    public DateTime SettlementDate { get; set; }
}