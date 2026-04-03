using Gvn.GvnAI.Dictionary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class WordRelationshipConfiguration : IEntityTypeConfiguration<WordRelationship>
{
    public void Configure(EntityTypeBuilder<WordRelationship> builder)
    {
        builder.ToTable("word_relationships");
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Type).IsRequired().HasConversion<string>().HasMaxLength(30);

        // FK relationships
        builder.HasOne<Word>().WithMany().HasForeignKey(r => r.SourceWordId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Word>().WithMany().HasForeignKey(r => r.TargetWordId).OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(r => new { r.SourceWordId, r.TargetWordId, r.Type }).IsUnique();
    }
}
