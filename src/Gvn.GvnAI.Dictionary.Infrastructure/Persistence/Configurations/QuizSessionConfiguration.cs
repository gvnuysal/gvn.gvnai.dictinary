using Gvn.GvnAI.Dictionary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class QuizSessionConfiguration : IEntityTypeConfiguration<QuizSession>
{
    public void Configure(EntityTypeBuilder<QuizSession> builder)
    {
        builder.ToTable("quiz_sessions");
        builder.HasKey(q => q.Id);

        builder.Property(q => q.UserId).IsRequired();
        builder.Property(q => q.TotalScore).IsRequired();
        builder.Property(q => q.CorrectCount).IsRequired();
        builder.Property(q => q.WrongCount).IsRequired();
        builder.Property(q => q.TotalQuestions).IsRequired();
        builder.Property(q => q.IsCompleted).HasDefaultValue(false);
        builder.Property(q => q.CompletedAt);

        // FK to User
        builder.HasOne<User>().WithMany().HasForeignKey(q => q.UserId).OnDelete(DeleteBehavior.Restrict);

        // Collections
        builder.HasMany(q => q.Answers)
            .WithOne()
            .HasForeignKey(a => a.QuizSessionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Navigation(q => q.Answers).UsePropertyAccessMode(PropertyAccessMode.Field);

        // Indexes
        builder.HasIndex(q => q.UserId);
        builder.HasIndex(q => q.CreatedAt);
    }
}
