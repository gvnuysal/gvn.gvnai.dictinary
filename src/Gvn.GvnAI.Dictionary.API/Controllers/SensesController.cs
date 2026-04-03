using Gvn.GvnAI.Dictionary.Application.Features.Senses.Commands.AddSense;
using Gvn.GvnAI.Dictionary.Application.Features.Senses.Commands.RemoveSense;
using Gvn.GvnAI.Dictionary.Application.Features.Senses.Commands.UpdateSense;
using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.AspNetCore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvn.GvnAI.Dictionary.API.Controllers;

[Route("api/words/{wordId:guid}/senses")]
public class SensesController(IMediator mediator) : ApiControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add(Guid wordId, [FromBody] AddSenseRequest request)
    {
        var result = await mediator.Send(new AddSenseCommand(
            wordId, request.Definition, request.DefinitionShort,
            request.RegisterId, request.DomainId,
            request.FrequencyRank, request.DifficultyLevel));
        return HandleResult(result);
    }

    [Authorize]
    [HttpPut("{senseId:guid}")]
    public async Task<IActionResult> Update(Guid wordId, Guid senseId, [FromBody] UpdateSenseRequest request)
    {
        var result = await mediator.Send(new UpdateSenseCommand(
            wordId, senseId, request.Definition, request.DefinitionShort,
            request.RegisterId, request.DomainId,
            request.FrequencyRank, request.DifficultyLevel));
        return HandleResult(result);
    }

    [Authorize]
    [HttpDelete("{senseId:guid}")]
    public async Task<IActionResult> Remove(Guid wordId, Guid senseId)
    {
        var result = await mediator.Send(new RemoveSenseCommand(wordId, senseId));
        return HandleResult(result);
    }
}

public sealed record AddSenseRequest(
    string Definition, string? DefinitionShort,
    Guid? RegisterId, Guid? DomainId,
    int? FrequencyRank, DifficultyLevel? DifficultyLevel);

public sealed record UpdateSenseRequest(
    string Definition, string? DefinitionShort,
    Guid? RegisterId, Guid? DomainId,
    int? FrequencyRank, DifficultyLevel? DifficultyLevel);
