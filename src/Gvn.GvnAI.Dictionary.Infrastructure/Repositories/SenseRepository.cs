using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Infrastructure.Persistence;
using Gvn.GvnFramework.EntityFramewokCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Repositories;

public class SenseRepository(DictionaryDbContext context)
    : EfRepository<Sense, DictionaryDbContext>(context), ISenseRepository
{
    public async Task<int> GetMaxSenseNumberAsync(Guid wordId, CancellationToken cancellationToken = default)
    {
        var max = await DbSet
            .Where(s => s.WordId == wordId)
            .MaxAsync(s => (int?)s.SenseNumber, cancellationToken);
        return max ?? 0;
    }

    public async Task<Sense?> GetByIdWithTranslationsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .Include(s => s.Translations)
            .Include(s => s.Examples)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }
}
