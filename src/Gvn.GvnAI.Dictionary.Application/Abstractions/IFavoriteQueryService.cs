using Gvn.GvnAI.Dictionary.Application.DTOs;

namespace Gvn.GvnAI.Dictionary.Application.Abstractions;

public interface IFavoriteQueryService
{
    Task<List<FavoriteWordDto>> GetFavoriteWordsAsync(Guid userId, CancellationToken ct = default);
}
