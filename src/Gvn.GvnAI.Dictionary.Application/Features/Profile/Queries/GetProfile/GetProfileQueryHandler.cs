using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;

namespace Gvn.GvnAI.Dictionary.Application.Features.Profile.Queries.GetProfile;

public sealed class GetProfileQueryHandler(
    IUserRepository userRepository,
    IProfileQueryService profileQueryService) : IQueryHandler<GetProfileQuery, ProfileDto>
{
    public async Task<Result<ProfileDto>> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
            return Result<ProfileDto>.Fail(Error.NotFound("USER_NOT_FOUND", "User not found."));

        var stats = await profileQueryService.GetUserStatsAsync(request.UserId, cancellationToken);

        return Result<ProfileDto>.Ok(new ProfileDto(
            user.Id, user.Email, user.FullName, user.Role,
            user.CreatedAt, stats));
    }
}
