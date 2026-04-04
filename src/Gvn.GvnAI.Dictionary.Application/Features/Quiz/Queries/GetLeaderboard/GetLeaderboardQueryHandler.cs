using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;

namespace Gvn.GvnAI.Dictionary.Application.Features.Quiz.Queries.GetLeaderboard;

public sealed class GetLeaderboardQueryHandler(
    IQuizService quizService) : IQueryHandler<GetLeaderboardQuery, List<LeaderboardEntryDto>>
{
    public async Task<Result<List<LeaderboardEntryDto>>> Handle(GetLeaderboardQuery request, CancellationToken cancellationToken)
    {
        var leaderboard = await quizService.GetLeaderboardAsync(request.Top, cancellationToken);
        return Result<List<LeaderboardEntryDto>>.Ok(leaderboard);
    }
}
