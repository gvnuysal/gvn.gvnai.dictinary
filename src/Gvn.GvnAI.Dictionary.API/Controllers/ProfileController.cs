using Gvn.GvnAI.Dictionary.Application.Features.Profile.Commands.UpdateApiSettings;
using Gvn.GvnAI.Dictionary.Application.Features.Profile.Commands.UpdateProfile;
using Gvn.GvnAI.Dictionary.Application.Features.Profile.Queries.GetProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvn.GvnAI.Dictionary.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ProfileController(IMediator mediator) : DictionaryControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var result = await mediator.Send(new GetProfileQuery(GetUserId()));
        return HandleResult(result);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var result = await mediator.Send(new UpdateProfileCommand(GetUserId(), request.FullName));
        return HandleResult(result);
    }

    [HttpPut("api-settings")]
    public async Task<IActionResult> UpdateApiSettings([FromBody] UpdateApiSettingsRequest request)
    {
        var result = await mediator.Send(new UpdateApiSettingsCommand(
            GetUserId(), request.TranslateProvider, request.ClaudeApiKey, request.GoogleTranslateApiKey, request.QuizAutoSpeak));
        return HandleResult(result);
    }

}

public record UpdateProfileRequest(string FullName);
public record UpdateApiSettingsRequest(string TranslateProvider, string? ClaudeApiKey, string? GoogleTranslateApiKey, bool QuizAutoSpeak = true);
