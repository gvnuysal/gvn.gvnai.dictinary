using Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.CreateWord;
using Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.CreateWordWithTranslation;
using Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.DeleteWord;
using Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.EnrichWord;
using Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.UpdateWord;
using Gvn.GvnAI.Dictionary.Application.Features.Words.Queries.GetWordById;
using Gvn.GvnAI.Dictionary.Application.Features.Words.Queries.GetWords;
using Gvn.GvnAI.Dictionary.Application.Features.Words.Queries.SearchWords;
using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.AspNetCore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvn.GvnAI.Dictionary.API.Controllers;

[Route("api/[controller]")]
public class WordsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? languageId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await mediator.Send(new GetWordsQuery(languageId, pageNumber, pageSize));
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetWordByIdQuery(id));
        return HandleResult(result);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? q,
        [FromQuery] Guid? languageId,
        [FromQuery] Guid? partOfSpeechId,
        [FromQuery] Guid? domainId,
        [FromQuery] Guid? registerId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await mediator.Send(new SearchWordsQuery(
            q, languageId, partOfSpeechId, domainId, registerId, pageNumber, pageSize));
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWordCommand command)
    {
        var result = await mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("with-translation")]
    public async Task<IActionResult> CreateWithTranslation([FromBody] CreateWordWithTranslationCommand command)
    {
        var result = await mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateWordCommand command)
    {
        if (id != command.Id)
            return BadRequest("Route id does not match body id.");

        var result = await mediator.Send(command);
        return HandleResult(result);
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteWordCommand(id));
        return HandleResult(result);
    }

    [Authorize]
    [HttpPost("{id:guid}/enrich")]
    public async Task<IActionResult> Enrich(Guid id, [FromQuery] string? targetLanguageCode)
    {
        var result = await mediator.Send(new EnrichWordCommand(id, targetLanguageCode));
        return HandleResult(result);
    }
}
