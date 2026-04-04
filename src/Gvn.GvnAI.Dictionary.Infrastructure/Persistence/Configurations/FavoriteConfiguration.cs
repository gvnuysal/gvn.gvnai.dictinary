using Gvn.GvnAI.Dictionary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Persistence.Configurations;

public class FavoriteConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("favorites");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.UserId).IsRequired();
        builder.Property(f => f.WordId).IsRequired();
        builder.Property(f => f.AddedAt).IsRequired();

        builder.HasOne<User>().WithMany().HasForeignKey(f => f.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasOne<Word>().WithMany().HasForeignKey(f => f.WordId).OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(f => new { f.UserId, f.WordId }).IsUnique();
        builder.HasIndex(f => f.UserId);
    }
}
