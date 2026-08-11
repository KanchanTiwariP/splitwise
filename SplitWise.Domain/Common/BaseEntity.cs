namespace SplitWise.Domain.Common;

public class BaseEntity
{
    public int Id {get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
}