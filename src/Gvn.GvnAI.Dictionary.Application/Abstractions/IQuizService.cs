using Gvn.GvnAI.Dictionary.Application.DTOs;

namespace Gvn.GvnAI.Dictionary.Application.Abstractions;

public interface IQuizService
{
    Task<(Guid WordId, string Lemma, string Definition, Guid CorrectTranslationId, string CorrectTranslationText)?> GetRandomWordWithTranslationAsync(
        Guid userId, IEnumerable<Guid> excludeWordIds, CancellationToken ct = default);

    Task<List<(Guid TranslationId, string Text)>> GetRandomWrongOptionsAsync(
        Guid userId, Guid excludeWordId, int count, CancellationToken ct = default);

    Task<List<QuizAnswerDetailDto>> GetAnswerDetailsBySessionAsync(
        Guid sessionId, CancellationToken ct = default);

    Task<List<Guid>> GetAskedWordIdsAsync(Guid sessionId, CancellationToken ct = default);

    Task<List<LeaderboardEntryDto>> GetLeaderboardAsync(int top, CancellationToken ct = default);
}
