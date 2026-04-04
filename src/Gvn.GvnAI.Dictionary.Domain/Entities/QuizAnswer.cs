using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class QuizAnswer : Entity
{
    public Guid QuizSessionId { get; private set; }
    public Guid WordId { get; private set; }
    public Guid? SelectedTranslationId { get; private set; }  // null = timeout
    public Guid CorrectTranslationId { get; private set; }
    public bool IsCorrect { get; private set; }
    public int PointsEarned { get; private set; }  // +5 or -3
    public int ResponseTimeMs { get; private set; }  // 0 = timeout
    public DateTime AnsweredAt { get; private set; }

    private QuizAnswer() { }

    public static QuizAnswer Create(Guid sessionId, Guid wordId, Guid? selectedTranslationId,
        Guid correctTranslationId, bool isCorrect, int pointsEarned, int responseTimeMs)
    {
        return new QuizAnswer
        {
            QuizSessionId = sessionId,
            WordId = wordId,
            SelectedTranslationId = selectedTranslationId,
            CorrectTranslationId = correctTranslationId,
            IsCorrect = isCorrect,
            PointsEarned = pointsEarned,
            ResponseTimeMs = responseTimeMs,
            AnsweredAt = DateTime.UtcNow
        };
    }
}
