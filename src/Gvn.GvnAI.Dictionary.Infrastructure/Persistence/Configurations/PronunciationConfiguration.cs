using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class PronunciationConfiguration : IEntityTypeConfiguration<Pronunciation>
{
    public void Configure(EntityTypeBuilder<Pronunciation> builder)
    {
        builder.ToTable("pronunciations");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.IpaTranscription).IsRequired().HasMaxLength(DictionaryConstants.MaxIpaLength);
        builder.Property(p => p.Variant).HasMaxLength(DictionaryConstants.MaxVariantLength);
        builder.Property(p => p.IsStandard).HasDefaultValue(false);

        // Indexes
        builder.HasIndex(p => p.WordId);
    }
}
