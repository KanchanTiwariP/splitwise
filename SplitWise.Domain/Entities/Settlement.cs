using SplitWise.Domain.Common;

namespace SplitWise.Domain.Entities;

public class Settlement :BaseEntity
{
    public int PayerId { get; set; }

    public int ReceiverId { get; set; }

    public int GroupId { get; set; }

    public decimal Amount { get; set; }

    public DateTime SettlementDate { get; set; }

    public User? Payer { get; set; } = null!;

    public User? Receiver { get; set; } = null!;

    public Group? Group { get; set; } = null!;
}