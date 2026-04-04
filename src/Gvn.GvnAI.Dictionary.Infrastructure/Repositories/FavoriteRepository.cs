using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Infrastructure.Persistence;
using Gvn.GvnFramework.EntityFramewokCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Repositories;

public class FavoriteRepository(DictionaryDbContext context)
    : EfRepository<Favorite, DictionaryDbContext>(context), IFavoriteRepository
{
    public async Task<bool> ExistsAsync(Guid userId, Guid wordId, CancellationToken cancellationToken = default)
    {
        return await DbSet.AnyAsync(f => f.UserId == userId && f.WordId == wordId, cancellationToken);
    }

    public async Task<Favorite?> GetByUserAndWordAsync(Guid userId, Guid wordId, CancellationToken cancellationToken = default)
    {
        return await DbSet.FirstOrDefaultAsync(f => f.UserId == userId && f.WordId == wordId, cancellationToken);
    }

    public async Task<List<Guid>> GetFavoriteWordIdsAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await DbSet.Where(f => f.UserId == userId).Select(f => f.WordId).ToListAsync(cancellationToken);
    }
}
