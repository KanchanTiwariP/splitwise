using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitWise.Domain.Entities;

namespace SplitWise.Infrastructure.Persistence.Configurations;

public class ExpenseSplitConfiguration :IEntityTypeConfiguration<ExpenseSplit>
{
    public void Configure(EntityTypeBuilder<ExpenseSplit> builder)
    {
        builder.ToTable("ExpenseSplits");

        builder.HasKey(x => new { x.ExpenseId, x.UserId });

        builder.Property(x => x.ShareAmount)
            .HasPrecision(18, 2);
        
        builder.HasOne(x => x.Expense)
            .WithMany(x => x.ExpenseSplits)
            .HasForeignKey(x => x.ExpenseId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.User)
            .WithMany(x => x.ExpenseSplits)
            .HasForeignKey(x=>x.UserId)    
            .OnDelete(DeleteBehavior.Restrict);
    }
}