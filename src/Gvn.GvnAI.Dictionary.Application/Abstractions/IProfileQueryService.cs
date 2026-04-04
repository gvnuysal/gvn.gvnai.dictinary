using Gvn.GvnAI.Dictionary.Application.DTOs;

namespace Gvn.GvnAI.Dictionary.Application.Abstractions;

public interface IProfileQueryService
{
    Task<ProfileStatsDto> GetUserStatsAsync(Guid userId, CancellationToken ct = default);
}
