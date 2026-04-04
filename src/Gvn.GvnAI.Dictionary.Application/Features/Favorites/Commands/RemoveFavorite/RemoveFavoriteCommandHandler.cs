using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Favorites.Commands.RemoveFavorite;

public sealed class RemoveFavoriteCommandHandler(
    IFavoriteRepository favoriteRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveFavoriteCommand>
{
    public async Task<Result> Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
    {
        var favorite = await favoriteRepository.GetByUserAndWordAsync(request.UserId, request.WordId, cancellationToken);
        if (favorite is not null)
        {
            await favoriteRepository.DeleteAsync(favorite, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return Result.Ok();
    }
}
