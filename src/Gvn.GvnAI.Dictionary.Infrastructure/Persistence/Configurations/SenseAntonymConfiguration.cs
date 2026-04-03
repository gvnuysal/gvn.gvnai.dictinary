using Gvn.GvnAI.Dictionary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class SenseAntonymConfiguration : IEntityTypeConfiguration<SenseAntonym>
{
    public void Configure(EntityTypeBuilder<SenseAntonym> builder)
    {
        builder.ToTable("sense_antonyms");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Strength).IsRequired().HasConversion<string>().HasMaxLength(20);

        // FK relationships
        builder.HasOne<Sense>().WithMany().HasForeignKey(a => a.SenseId1).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Sense>().WithMany().HasForeignKey(a => a.SenseId2).OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(a => new { a.SenseId1, a.SenseId2 }).IsUnique();
    }
}
