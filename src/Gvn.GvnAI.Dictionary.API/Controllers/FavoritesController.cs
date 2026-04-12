using Gvn.GvnAI.Dictionary.Application.Features.Favorites.Commands.AddFavorite;
using Gvn.GvnAI.Dictionary.Application.Features.Favorites.Commands.RemoveFavorite;
using Gvn.GvnAI.Dictionary.Application.Features.Favorites.Queries.GetFavorites;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvn.GvnAI.Dictionary.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class FavoritesController(IMediator mediator) : DictionaryControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetFavorites()
    {
        var result = await mediator.Send(new GetFavoritesQuery(GetUserId()));
        return HandleResult(result);
    }

    [HttpPost("{wordId:guid}")]
    public async Task<IActionResult> Add(Guid wordId)
    {
        var result = await mediator.Send(new AddFavoriteCommand(GetUserId(), wordId));
        return HandleResult(result);
    }

    [HttpDelete("{wordId:guid}")]
    public async Task<IActionResult> Remove(Guid wordId)
    {
        var result = await mediator.Send(new RemoveFavoriteCommand(GetUserId(), wordId));
        return HandleResult(result);
    }

    [HttpGet("{wordId:guid}/check")]
    public async Task<IActionResult> Check(Guid wordId)
    {
        var result = await mediator.Send(new GetFavoritesQuery(GetUserId()));
        return result.Match<IActionResult>(
            favorites => Ok(new { isFavorite = favorites.Any(f => f.WordId == wordId) }),
            errors => HandleResult(result));
    }

}
