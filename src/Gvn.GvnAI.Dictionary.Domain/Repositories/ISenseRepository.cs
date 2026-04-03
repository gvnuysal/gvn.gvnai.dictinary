using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Domain.Repositories;

public interface ISenseRepository : IRepository<Sense>
{
    Task<int> GetMaxSenseNumberAsync(Guid wordId, CancellationToken cancellationToken = default);
    Task<Sense?> GetByIdWithTranslationsAsync(Guid id, CancellationToken cancellationToken = default);
}
