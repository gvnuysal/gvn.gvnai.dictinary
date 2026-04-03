using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class WordConfiguration : IEntityTypeConfiguration<Word>
{
    public void Configure(EntityTypeBuilder<Word> builder)
    {
        builder.ToTable("words");
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Lemma).IsRequired().HasMaxLength(DictionaryConstants.MaxLemmaLength);
        builder.Property(w => w.LanguageId).IsRequired();
        builder.Property(w => w.PartOfSpeechId).IsRequired();
        builder.Property(w => w.FrequencyRank);
        builder.Property(w => w.DifficultyLevel).HasConversion<string>().HasMaxLength(5);
        builder.Property(w => w.IsCompound).HasDefaultValue(false);
        builder.Property(w => w.IsIdiom).HasDefaultValue(false);
        builder.Property(w => w.IsProperNoun).HasDefaultValue(false);
        builder.Property(w => w.Status).IsRequired().HasConversion<string>().HasMaxLength(20);

        // ISoftDeletable
        builder.Property(w => w.IsDeleted).HasDefaultValue(false);
        builder.Property(w => w.DeletedAt);
        builder.Property(w => w.DeletedBy).HasMaxLength(200);

        // Indexes
        builder.HasIndex(w => new { w.Lemma, w.LanguageId }).IsUnique().HasFilter("\"IsDeleted\" = false");
        builder.HasIndex(w => w.Status);
        builder.HasIndex(w => w.LanguageId);

        // FK relationships
        builder.HasOne<Language>().WithMany().HasForeignKey(w => w.LanguageId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<PartOfSpeech>().WithMany().HasForeignKey(w => w.PartOfSpeechId).OnDelete(DeleteBehavior.Restrict);

        // Collections
        builder.HasMany(w => w.Senses).WithOne().HasForeignKey(s => s.WordId).OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(w => w.Senses).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(w => w.Pronunciations).WithOne().HasForeignKey(p => p.WordId).OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(w => w.Pronunciations).UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(w => w.Etymologies).WithOne().HasForeignKey(e => e.WordId).OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(w => w.Etymologies).UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}
