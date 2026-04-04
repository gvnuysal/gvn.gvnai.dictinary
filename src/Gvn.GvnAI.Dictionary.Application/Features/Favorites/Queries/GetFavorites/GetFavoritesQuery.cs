using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Favorites.Queries.GetFavorites;

public sealed record GetFavoritesQuery(Guid UserId) : IQuery<List<FavoriteWordDto>>;
