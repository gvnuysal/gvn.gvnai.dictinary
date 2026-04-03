using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Infrastructure.Persistence;
using Gvn.GvnFramework.EntityFramewokCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Repositories;

public class PartOfSpeechRepository(DictionaryDbContext context)
    : EfRepository<PartOfSpeech, DictionaryDbContext>(context), IPartOfSpeechRepository
{
    public async Task<PartOfSpeech?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
    {
        return await DbSet
            .FirstOrDefaultAsync(p => p.Code == code.ToUpperInvariant(), cancellationToken);
    }
}
