using Gvn.GvnAI.Dictionary.Application.Features.Lookups.Queries;
using Gvn.GvnFramework.AspNetCore.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Gvn.GvnAI.Dictionary.API.Controllers;

[Route("api/[controller]")]
public class LookupsController(IMediator mediator) : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await mediator.Send(new GetLookupsQuery());
        return HandleResult(result);
    }
}
