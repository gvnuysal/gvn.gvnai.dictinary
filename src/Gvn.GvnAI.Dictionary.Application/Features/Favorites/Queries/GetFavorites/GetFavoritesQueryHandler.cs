using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;

namespace Gvn.GvnAI.Dictionary.Application.Features.Favorites.Queries.GetFavorites;

public sealed class GetFavoritesQueryHandler(
    IFavoriteQueryService favoriteQueryService) : IQueryHandler<GetFavoritesQuery, List<FavoriteWordDto>>
{
    public async Task<Result<List<FavoriteWordDto>>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
    {
        var favorites = await favoriteQueryService.GetFavoriteWordsAsync(request.UserId, cancellationToken);
        return Result<List<FavoriteWordDto>>.Ok(favorites);
    }
}
