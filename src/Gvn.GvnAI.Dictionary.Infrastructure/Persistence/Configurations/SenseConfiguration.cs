using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class SenseConfiguration : IEntityTypeConfiguration<Sense>
{
    public void Configure(EntityTypeBuilder<Sense> builder)
    {
        builder.ToTable("senses");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Definition).IsRequired().HasMaxLength(DictionaryConstants.MaxDefinitionLength);
        builder.Property(s => s.DefinitionShort).HasMaxLength(DictionaryConstants.MaxDefinitionShortLength);
        builder.Property(s => s.SenseNumber).IsRequired();
        builder.Property(s => s.RegisterId);
        builder.Property(s => s.DomainId);
        builder.Property(s => s.FrequencyRank);
        builder.Property(s => s.DifficultyLevel).HasConversion<string>().HasMaxLength(5);

        // FK relationships
        builder.HasOne<Register>().WithMany().HasForeignKey(s => s.RegisterId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne<SubjectDomain>().WithMany().HasForeignKey(s => s.DomainId).OnDelete(DeleteBehavior.SetNull);

        // Collections
        builder.HasMany(s => s.Translations).WithOne().HasForeignKey(t => t.SenseId).OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(s => s.Translations).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(s => s.Examples).WithOne().HasForeignKey(e => e.SenseId).OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(s => s.Examples).UsePropertyAccessMode(PropertyAccessMode.Field);

        // Indexes
        builder.HasIndex(s => new { s.WordId, s.SenseNumber }).IsUnique();
    }
}
