using System.Security.Claims;
using Gvn.GvnFramework.AspNetCore.Controllers;

namespace Gvn.GvnAI.Dictionary.API.Controllers;

public abstract class DictionaryControllerBase : ApiControllerBase
{
    protected Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (claim is null || !Guid.TryParse(claim.Value, out var userId))
            throw new UnauthorizedAccessException("User id claim missing or invalid.");
        return userId;
    }
}
