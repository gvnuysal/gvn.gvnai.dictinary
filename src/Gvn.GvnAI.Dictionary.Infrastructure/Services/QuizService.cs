using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnAI.Dictionary.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Services;

public class QuizService(DictionaryDbContext dbContext) : IQuizService
{
    public async Task<(Guid WordId, string Lemma, string Definition, Guid CorrectTranslationId, string CorrectTranslationText)?> GetRandomWordWithTranslationAsync(
        Guid userId, IEnumerable<Guid> excludeWordIds, CancellationToken ct = default)
    {
        var excludeList = excludeWordIds.ToList();

        var candidate = await GetRandomCandidateAsync(userId, excludeList, ct);

        if (candidate is null && excludeList.Count > 0)
            candidate = await GetRandomCandidateAsync(userId, [], ct);

        if (candidate is null)
            return null;

        return (candidate.Id, candidate.Lemma, candidate.Definition, candidate.Translation.Id, candidate.Translation.TranslationText);
    }

    private async Task<CandidateWord?> GetRandomCandidateAsync(Guid userId, List<Guid> excludeList, CancellationToken ct)
    {
        return await dbContext.Words
            .Where(w => w.UserId == userId)
            .Where(w => w.Status != WordStatus.Pending && w.Status != WordStatus.Failed)
            .Where(w => !excludeList.Contains(w.Id))
            .Where(w => w.Senses.Any(s => s.Translations.Any()))
            .OrderBy(w => EF.Functions.Random())
            .Select(w => new CandidateWord
            {
                Id = w.Id,
                Lemma = w.Lemma,
                Definition = w.Senses.OrderBy(s => s.SenseNumber).First().Definition,
                Translation = w.Senses
                    .OrderBy(s => s.SenseNumber)
                    .SelectMany(s => s.Translations)
                    .Select(t => new CandidateTranslation { Id = t.Id, TranslationText = t.TranslationText })
                    .First()
            })
            .FirstOrDefaultAsync(ct);
    }

    private class CandidateWord
    {
        public Guid Id { get; set; }
        public string Lemma { get; set; } = null!;
        public string Definition { get; set; } = null!;
        public CandidateTranslation Translation { get; set; } = null!;
    }

    private class CandidateTranslation
    {
        public Guid Id { get; set; }
        public string TranslationText { get; set; } = null!;
    }

    public async Task<List<(Guid TranslationId, string Text)>> GetRandomWrongOptionsAsync(
        Guid userId, Guid excludeWordId, int count, CancellationToken ct = default)
    {
        var wrongOptions = await dbContext.Translations
            .Where(t => dbContext.Senses
                .Where(s => s.WordId != excludeWordId)
                .Where(s => dbContext.Words.Where(w => w.UserId == userId).Select(w => w.Id).Contains(s.WordId))
                .Select(s => s.Id)
                .Contains(t.SenseId))
            .OrderBy(t => EF.Functions.Random())
            .Select(t => new { t.Id, t.TranslationText })
            .Take(count)
            .ToListAsync(ct);

        return wrongOptions.Select(o => (o.Id, o.TranslationText)).ToList();
    }

    public async Task<List<QuizAnswerDetailDto>> GetAnswerDetailsBySessionAsync(
        Guid sessionId, CancellationToken ct = default)
    {
        var answerList = await dbContext.QuizAnswers
            .Where(a => a.QuizSessionId == sessionId)
            .OrderBy(a => a.AnsweredAt)
            .ToListAsync(ct);

        var wordIds = answerList.Select(a => a.WordId).Distinct().ToList();
        var translationIds = answerList
            .SelectMany(a => new[] { a.CorrectTranslationId, a.SelectedTranslationId ?? Guid.Empty })
            .Where(id => id != Guid.Empty)
            .Distinct()
            .ToList();

        var words = await dbContext.Words
            .Where(w => wordIds.Contains(w.Id))
            .Select(w => new { w.Id, w.Lemma })
            .ToDictionaryAsync(w => w.Id, w => w.Lemma, ct);

        var translations = await dbContext.Translations
            .Where(t => translationIds.Contains(t.Id))
            .Select(t => new { t.Id, t.TranslationText })
            .ToDictionaryAsync(t => t.Id, t => t.TranslationText, ct);

        return answerList.Select(a => new QuizAnswerDetailDto(
            words.GetValueOrDefault(a.WordId, "—"),
            translations.GetValueOrDefault(a.CorrectTranslationId, "—"),
            a.SelectedTranslationId.HasValue
                ? translations.GetValueOrDefault(a.SelectedTranslationId.Value, "—")
                : null,
            a.IsCorrect,
            a.PointsEarned,
            a.ResponseTimeMs
        )).ToList();
    }

    public async Task<List<Guid>> GetAskedWordIdsAsync(Guid sessionId, CancellationToken ct = default)
    {
        return await dbContext.QuizAnswers
            .Where(a => a.QuizSessionId == sessionId)
            .Select(a => a.WordId)
            .ToListAsync(ct);
    }

    public async Task<List<LeaderboardEntryDto>> GetLeaderboardAsync(int top, CancellationToken ct = default)
    {
        var leaderboard = await dbContext.QuizSessions
            .Where(q => q.IsCompleted)
            .GroupBy(q => q.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                TotalScore = g.Sum(q => q.TotalScore),
                GamesPlayed = g.Count(),
                TotalCorrect = g.Sum(q => q.CorrectCount),
                TotalQuestions = g.Sum(q => q.TotalQuestions)
            })
            .OrderByDescending(x => x.TotalScore)
            .Take(top)
            .ToListAsync(ct);

        var userIds = leaderboard.Select(l => l.UserId).ToList();
        var users = await dbContext.Users
            .Where(u => userIds.Contains(u.Id))
            .Select(u => new { u.Id, u.FullName })
            .ToDictionaryAsync(u => u.Id, u => u.FullName, ct);

        return leaderboard.Select(l => new LeaderboardEntryDto(
            users.GetValueOrDefault(l.UserId, "Unknown"),
            l.TotalScore,
            l.GamesPlayed,
            l.TotalQuestions > 0 ? Math.Round((double)l.TotalCorrect / l.TotalQuestions * 100, 2) : 0
        )).ToList();
    }
}
