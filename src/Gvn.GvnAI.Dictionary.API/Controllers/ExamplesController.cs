using Gvn.GvnAI.Dictionary.Application.Features.Examples.Commands.AddExample;
using Gvn.GvnAI.Dictionary.Application.Features.Examples.Commands.RemoveExample;
using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.AspNetCore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvn.GvnAI.Dictionary.API.Controllers;

[Route("api/words/{wordId:guid}/senses/{senseId:guid}/examples")]
public class ExamplesController(IMediator mediator) : ApiControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add(Guid wordId, Guid senseId, [FromBody] AddExampleRequest request)
    {
        var result = await mediator.Send(new AddExampleCommand(
            wordId, senseId, request.TranslationId,
            request.SourceText, request.TargetText, request.Source));
        return HandleResult(result);
    }

    [Authorize]
    [HttpDelete("{exampleId:guid}")]
    public async Task<IActionResult> Remove(Guid wordId, Guid senseId, Guid exampleId)
    {
        var result = await mediator.Send(new RemoveExampleCommand(wordId, senseId, exampleId));
        return HandleResult(result);
    }
}

public sealed record AddExampleRequest(
    Guid? TranslationId,
    string SourceText,
    string? TargetText,
    ExampleSource Source);
