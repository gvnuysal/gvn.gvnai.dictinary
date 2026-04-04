using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Favorites.Commands.AddFavorite;

public sealed class AddFavoriteCommandHandler(
    IFavoriteRepository favoriteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddFavoriteCommand>
{
    public async Task<Result> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
    {
        var exists = await favoriteRepository.ExistsAsync(request.UserId, request.WordId, cancellationToken);
        if (exists)
            return Result.Ok();

        var favorite = Favorite.Create(request.UserId, request.WordId);
        await favoriteRepository.AddAsync(favorite, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
