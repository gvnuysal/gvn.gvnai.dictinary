using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Domain.Repositories;

public interface IFavoriteRepository : IRepository<Favorite>
{
    Task<bool> ExistsAsync(Guid userId, Guid wordId, CancellationToken cancellationToken = default);
    Task<Favorite?> GetByUserAndWordAsync(Guid userId, Guid wordId, CancellationToken cancellationToken = default);
    Task<List<Guid>> GetFavoriteWordIdsAsync(Guid userId, CancellationToken cancellationToken = default);
}
