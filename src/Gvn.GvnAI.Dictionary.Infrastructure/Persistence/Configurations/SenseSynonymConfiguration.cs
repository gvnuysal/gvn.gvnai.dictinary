using Gvn.GvnAI.Dictionary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class SenseSynonymConfiguration : IEntityTypeConfiguration<SenseSynonym>
{
    public void Configure(EntityTypeBuilder<SenseSynonym> builder)
    {
        builder.ToTable("sense_synonyms");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Strength).IsRequired().HasConversion<string>().HasMaxLength(20);

        // FK relationships
        builder.HasOne<Sense>().WithMany().HasForeignKey(s => s.SenseId1).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Sense>().WithMany().HasForeignKey(s => s.SenseId2).OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(s => new { s.SenseId1, s.SenseId2 }).IsUnique();
    }
}
