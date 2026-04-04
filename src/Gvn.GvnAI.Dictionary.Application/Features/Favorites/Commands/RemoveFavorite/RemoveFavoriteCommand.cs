using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Favorites.Commands.RemoveFavorite;

public sealed record RemoveFavoriteCommand(Guid UserId, Guid WordId) : ICommand;
