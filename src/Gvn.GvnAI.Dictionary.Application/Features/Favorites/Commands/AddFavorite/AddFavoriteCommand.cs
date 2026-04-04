using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Favorites.Commands.AddFavorite;

public sealed record AddFavoriteCommand(Guid UserId, Guid WordId) : ICommand;
