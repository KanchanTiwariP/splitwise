using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitWise.Domain.Entities;

namespace SplitWise.Infrastructure.Persistence.Configurations;

public class SettlementConfiguration : IEntityTypeConfiguration<Settlement>
{
    public void Configure(EntityTypeBuilder<Settlement> builder)
    {
         builder.ToTable("Settlements");
         builder.HasKey(x => x.Id);
         builder.Property(x => x.Amount)
             .HasPrecision(18, 2);
         builder.HasOne(x=>x.Group)
             .WithMany(x=>x.Settlements)
             .HasForeignKey(x=>x.GroupId)
             .OnDelete(DeleteBehavior.Cascade);
         
         builder.HasOne(x => x.Payer)
             .WithMany(x => x.PaymentsMade)
             .HasForeignKey(x => x.PayerId)
             .OnDelete(DeleteBehavior.Restrict);
         
         builder.HasOne(x => x.Receiver)
             .WithMany(x => x.PaymentsReceived)
             .HasForeignKey(x => x.ReceiverId)
             .OnDelete(DeleteBehavior.Restrict);
    }
}