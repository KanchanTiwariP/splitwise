using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitWise.Domain.Entities;

namespace SplitWise.Infrastructure.Persistence.Configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.ToTable("Expense");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(500);
        builder.Property(x => x.TotalAmount)
            .HasPrecision(18, 2);
        builder.Property(x => x.SplitType)
            .IsRequired();
        builder.HasOne(x => x.Group)
            .WithMany(x => x.Expenses)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);;
        
        builder.HasOne(x=>x.Payer)
            .WithMany(x=>x.PaidExpenses)
            .HasForeignKey(x=>x.PaidBy)
            .OnDelete(DeleteBehavior.Restrict);;
       
    }
}