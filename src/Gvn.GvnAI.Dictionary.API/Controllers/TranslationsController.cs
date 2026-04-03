using Gvn.GvnAI.Dictionary.Application.Features.Translations.Commands.AddTranslation;
using Gvn.GvnAI.Dictionary.Application.Features.Translations.Commands.RemoveTranslation;
using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.AspNetCore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvn.GvnAI.Dictionary.API.Controllers;

[Route("api/words/{wordId:guid}/senses/{senseId:guid}/translations")]
public class TranslationsController(IMediator mediator) : ApiControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Add(Guid wordId, Guid senseId, [FromBody] AddTranslationRequest request)
    {
        var result = await mediator.Send(new AddTranslationCommand(
            wordId, senseId, request.TargetLanguageId,
            request.TranslationText, request.PartOfSpeechId, request.RegisterId,
            request.EquivalenceType, request.ConfidenceScore));
        return HandleResult(result);
    }

    [Authorize]
    [HttpDelete("{translationId:guid}")]
    public async Task<IActionResult> Remove(Guid wordId, Guid senseId, Guid translationId)
    {
        var result = await mediator.Send(new RemoveTranslationCommand(wordId, senseId, translationId));
        return HandleResult(result);
    }
}

public sealed record AddTranslationRequest(
    Guid TargetLanguageId,
    string TranslationText,
    Guid? PartOfSpeechId,
    Guid? RegisterId,
    EquivalenceType EquivalenceType,
    double ConfidenceScore);
