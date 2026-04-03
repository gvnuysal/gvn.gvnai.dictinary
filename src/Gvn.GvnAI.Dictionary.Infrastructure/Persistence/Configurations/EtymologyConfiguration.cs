using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class EtymologyConfiguration : IEntityTypeConfiguration<Etymology>
{
    public void Configure(EntityTypeBuilder<Etymology> builder)
    {
        builder.ToTable("etymologies");
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Text).IsRequired().HasMaxLength(DictionaryConstants.MaxEtymologyLength);

        // FK relationships - WordId FK is configured in WordConfiguration (cascade)
        builder.HasOne<Language>().WithMany().HasForeignKey(e => e.OriginLanguageId).OnDelete(DeleteBehavior.SetNull);
    }
}
