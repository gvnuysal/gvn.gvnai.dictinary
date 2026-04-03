using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class PartOfSpeechConfiguration : IEntityTypeConfiguration<PartOfSpeech>
{
    public void Configure(EntityTypeBuilder<PartOfSpeech> builder)
    {
        builder.ToTable("parts_of_speech");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Code).IsRequired().HasMaxLength(DictionaryConstants.MaxCodeLength);
        builder.Property(p => p.Name).IsRequired().HasMaxLength(DictionaryConstants.MaxNameLength);
        builder.Property(p => p.Abbreviation).IsRequired().HasMaxLength(DictionaryConstants.MaxAbbreviationLength);
        builder.HasIndex(p => p.Code).IsUnique();
    }
}
