using Gvn.GvnAI.Dictionary.Application.Features.Auth.Commands.Login;
using Gvn.GvnAI.Dictionary.Application.Features.Auth.Commands.Register;
using Gvn.GvnFramework.AspNetCore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gvn.GvnAI.Dictionary.API.Controllers;

[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ApiControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await mediator.Send(command);
        return HandleResult(result);
    }
}
