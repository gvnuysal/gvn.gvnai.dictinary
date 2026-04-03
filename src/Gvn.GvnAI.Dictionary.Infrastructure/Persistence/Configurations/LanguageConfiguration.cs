using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class LanguageConfiguration : IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("languages");
        builder.HasKey(l => l.Id);
        builder.Property(l => l.Code).IsRequired().HasMaxLength(DictionaryConstants.MaxCodeLength);
        builder.Property(l => l.Name).IsRequired().HasMaxLength(DictionaryConstants.MaxNameLength);
        builder.Property(l => l.NativeName).IsRequired().HasMaxLength(DictionaryConstants.MaxNativeNameLength);
        builder.Property(l => l.Direction).IsRequired().HasConversion<string>().HasMaxLength(5);
        builder.HasIndex(l => l.Code).IsUnique();
    }
}
