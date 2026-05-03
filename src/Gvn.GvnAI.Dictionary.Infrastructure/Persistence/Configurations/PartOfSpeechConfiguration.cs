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

        builder.HasData(
            new { Id = new Guid("11111111-0000-0000-0000-000000000001"), Code = "NOUN",   Name = "İsim",   Abbreviation = "n."     },
            new { Id = new Guid("11111111-0000-0000-0000-000000000002"), Code = "VERB",   Name = "Fiil",   Abbreviation = "v."     },
            new { Id = new Guid("11111111-0000-0000-0000-000000000003"), Code = "ADJ",    Name = "Sıfat",  Abbreviation = "adj."   },
            new { Id = new Guid("11111111-0000-0000-0000-000000000004"), Code = "ADV",    Name = "Zarf",   Abbreviation = "adv."   },
            new { Id = new Guid("11111111-0000-0000-0000-000000000005"), Code = "PRON",   Name = "Zamir",  Abbreviation = "pron."  },
            new { Id = new Guid("11111111-0000-0000-0000-000000000006"), Code = "PREP",   Name = "Edat",   Abbreviation = "prep."  },
            new { Id = new Guid("11111111-0000-0000-0000-000000000007"), Code = "CONJ",   Name = "Bağlaç", Abbreviation = "conj."  },
            new { Id = new Guid("11111111-0000-0000-0000-000000000008"), Code = "INTERJ", Name = "Ünlem",  Abbreviation = "interj."}
        );
    }
}
