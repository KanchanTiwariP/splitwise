using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SplitWise.Domain.Entities;

namespace SplitWise.Infrastructure.Persistence.Configurations;

public class GroupMemberConfiguration : IEntityTypeConfiguration<GroupMember>
{
    public void Configure(EntityTypeBuilder<GroupMember> builder)
    {
        builder.ToTable("GroupMember");
        builder.HasKey(x => new {x.GroupId, x.UserId});
        
        builder.HasOne(x => x.Group)
            .WithMany(x => x.Members)
            .HasForeignKey(x => x.GroupId)
            .OnDelete(DeleteBehavior.Cascade);;
        

        builder.HasOne(x => x.User)
            .WithMany(x => x.GroupMembers)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}