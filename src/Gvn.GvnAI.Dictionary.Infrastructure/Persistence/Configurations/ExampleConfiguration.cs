using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class ExampleConfiguration : IEntityTypeConfiguration<Example>
{
    public void Configure(EntityTypeBuilder<Example> builder)
    {
        builder.ToTable("examples");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.SourceText).IsRequired().HasMaxLength(DictionaryConstants.MaxSourceTextLength);
        builder.Property(e => e.TargetText).HasMaxLength(DictionaryConstants.MaxTargetTextLength);
        builder.Property(e => e.Source).HasConversion<string>().HasMaxLength(20);

        // FK relationships - SenseId FK is configured in SenseConfiguration (cascade)
        builder.HasOne<Translation>().WithMany().HasForeignKey(e => e.TranslationId).OnDelete(DeleteBehavior.SetNull);
    }
}
