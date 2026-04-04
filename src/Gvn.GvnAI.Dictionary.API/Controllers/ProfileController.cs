using System.Security.Claims;
using Gvn.GvnAI.Dictionary.Application.Features.Profile.Commands.UpdateProfile;
using Gvn.GvnAI.Dictionary.Application.Features.Profile.Queries.GetProfile;
using Gvn.GvnFramework.AspNetCore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvn.GvnAI.Dictionary.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ProfileController(IMediator mediator) : ApiControllerBase
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

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return Guid.Parse(claim!.Value);
    }
}

public record UpdateProfileRequest(string FullName);
