using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnAI.Dictionary.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Services;

public class ProfileQueryService(DictionaryDbContext dbContext) : IProfileQueryService
{
    public async Task<ProfileStatsDto> GetUserStatsAsync(Guid userId, CancellationToken ct = default)
    {
        var quizStats = await dbContext.QuizSessions
            .Where(q => q.UserId == userId && q.IsCompleted)
            .GroupBy(_ => 1)
            .Select(g => new
            {
                GamesPlayed = g.Count(),
                TotalScore = g.Sum(q => q.TotalScore),
                TotalCorrect = g.Sum(q => q.CorrectCount),
                TotalWrong = g.Sum(q => q.WrongCount),
                TotalQuestions = g.Sum(q => q.TotalQuestions),
                BestScore = g.Max(q => q.TotalScore)
            })
            .FirstOrDefaultAsync(ct);

        var favoriteCount = await dbContext.Favorites
            .CountAsync(f => f.UserId == userId, ct);

        var gamesPlayed = quizStats?.GamesPlayed ?? 0;
        var totalCorrect = quizStats?.TotalCorrect ?? 0;
        var totalQuestions = quizStats?.TotalQuestions ?? 0;
        var accuracy = totalQuestions > 0
            ? Math.Round((double)totalCorrect / totalQuestions * 100, 1)
            : 0;

        return new ProfileStatsDto(
            gamesPlayed,
            quizStats?.TotalScore ?? 0,
            totalCorrect,
            quizStats?.TotalWrong ?? 0,
            totalQuestions,
            accuracy,
            favoriteCount,
            quizStats?.BestScore ?? 0);
    }
}
