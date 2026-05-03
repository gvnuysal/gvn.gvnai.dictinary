using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;
using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
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

        builder.HasData(
            new { Id = new Guid("22222222-0000-0000-0000-000000000001"), Code = "en", Name = "English",    NativeName = "English",    Direction = TextDirection.LTR },
            new { Id = new Guid("22222222-0000-0000-0000-000000000002"), Code = "tr", Name = "Turkish",    NativeName = "Türkçe",     Direction = TextDirection.LTR },
            new { Id = new Guid("22222222-0000-0000-0000-000000000003"), Code = "de", Name = "German",     NativeName = "Deutsch",    Direction = TextDirection.LTR },
            new { Id = new Guid("22222222-0000-0000-0000-000000000004"), Code = "fr", Name = "French",     NativeName = "Français",   Direction = TextDirection.LTR },
            new { Id = new Guid("22222222-0000-0000-0000-000000000005"), Code = "es", Name = "Spanish",    NativeName = "Español",    Direction = TextDirection.LTR },
            new { Id = new Guid("22222222-0000-0000-0000-000000000006"), Code = "it", Name = "Italian",    NativeName = "Italiano",   Direction = TextDirection.LTR },
            new { Id = new Guid("22222222-0000-0000-0000-000000000007"), Code = "ru", Name = "Russian",    NativeName = "Русский",    Direction = TextDirection.LTR },
            new { Id = new Guid("22222222-0000-0000-0000-000000000008"), Code = "ar", Name = "Arabic",     NativeName = "العربية",    Direction = TextDirection.RTL },
            new { Id = new Guid("22222222-0000-0000-0000-000000000009"), Code = "ja", Name = "Japanese",   NativeName = "日本語",      Direction = TextDirection.LTR },
            new { Id = new Guid("22222222-0000-0000-0000-00000000000a"), Code = "zh", Name = "Chinese",    NativeName = "中文",        Direction = TextDirection.LTR }
        );
    }
}
