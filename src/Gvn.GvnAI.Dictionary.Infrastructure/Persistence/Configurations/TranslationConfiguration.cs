using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class TranslationConfiguration : IEntityTypeConfiguration<Translation>
{
    public void Configure(EntityTypeBuilder<Translation> builder)
    {
        builder.ToTable("translations");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.TranslationText).IsRequired().HasMaxLength(DictionaryConstants.MaxTranslationTextLength);
        builder.Property(t => t.EquivalenceType).HasConversion<string>().HasMaxLength(20);
        builder.Property(t => t.ConfidenceScore).IsRequired();

        // FK relationships
        builder.HasOne<Language>().WithMany().HasForeignKey(t => t.TargetLanguageId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<PartOfSpeech>().WithMany().HasForeignKey(t => t.PartOfSpeechId).OnDelete(DeleteBehavior.SetNull);
        builder.HasOne<Register>().WithMany().HasForeignKey(t => t.RegisterId).OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(t => new { t.SenseId, t.TargetLanguageId });
    }
}
