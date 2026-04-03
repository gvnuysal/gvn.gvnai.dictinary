using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class SubjectDomainConfiguration : IEntityTypeConfiguration<SubjectDomain>
{
    public void Configure(EntityTypeBuilder<SubjectDomain> builder)
    {
        builder.ToTable("domains");
        builder.HasKey(d => d.Id);
        builder.Property(d => d.Code).IsRequired().HasMaxLength(DictionaryConstants.MaxCodeLength);
        builder.Property(d => d.Name).IsRequired().HasMaxLength(DictionaryConstants.MaxNameLength);
        builder.HasIndex(d => d.Code).IsUnique();
    }
}
