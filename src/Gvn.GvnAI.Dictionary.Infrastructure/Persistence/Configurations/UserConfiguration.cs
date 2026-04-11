using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(DictionaryConstants.MaxEmailLength);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(DictionaryConstants.MaxFullNameLength);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.TranslateProvider)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("claude");

        builder.Property(u => u.ClaudeApiKey)
            .HasMaxLength(500);

        builder.Property(u => u.GoogleTranslateApiKey)
            .HasMaxLength(500);

        builder.Property(u => u.QuizAutoSpeak)
            .HasDefaultValue(true);

        builder.HasIndex(u => u.Email).IsUnique();
    }
}
