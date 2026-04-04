using Gvn.GvnAI.Dictionary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class QuizAnswerConfiguration : IEntityTypeConfiguration<QuizAnswer>
{
    public void Configure(EntityTypeBuilder<QuizAnswer> builder)
    {
        builder.ToTable("quiz_answers");
        builder.HasKey(a => a.Id);

        builder.Property(a => a.QuizSessionId).IsRequired();
        builder.Property(a => a.WordId).IsRequired();
        builder.Property(a => a.SelectedTranslationId);
        builder.Property(a => a.CorrectTranslationId).IsRequired();
        builder.Property(a => a.IsCorrect).IsRequired();
        builder.Property(a => a.PointsEarned).IsRequired();
        builder.Property(a => a.ResponseTimeMs).IsRequired();
        builder.Property(a => a.AnsweredAt).IsRequired();

        // FK to Word
        builder.HasOne<Word>().WithMany().HasForeignKey(a => a.WordId).OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(a => a.QuizSessionId);
        builder.HasIndex(a => a.WordId);
    }
}
