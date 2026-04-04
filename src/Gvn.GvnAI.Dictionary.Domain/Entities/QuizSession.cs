using Gvn.GvnFramework.Domain.Aggregates;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class QuizSession : AggregateRoot
{
    private readonly List<QuizAnswer> _answers = [];

    public Guid UserId { get; private set; }
    public int TotalScore { get; private set; }
    public int CorrectCount { get; private set; }
    public int WrongCount { get; private set; }
    public int TotalQuestions { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    public IReadOnlyCollection<QuizAnswer> Answers => _answers.AsReadOnly();

    private QuizSession() { }

    public static QuizSession Create(Guid userId)
    {
        return new QuizSession { UserId = userId };
    }

    public void AddAnswer(Guid wordId, Guid? selectedTranslationId, Guid correctTranslationId,
        bool isCorrect, int pointsEarned, int responseTimeMs)
    {
        _answers.Add(QuizAnswer.Create(Id, wordId, selectedTranslationId, correctTranslationId, isCorrect, pointsEarned, responseTimeMs));
        TotalQuestions++;
        TotalScore += pointsEarned;
        if (isCorrect) CorrectCount++;
        else WrongCount++;
    }

    public void RecordAnswer(bool isCorrect, int pointsEarned)
    {
        TotalQuestions++;
        TotalScore += pointsEarned;
        if (isCorrect) CorrectCount++;
        else WrongCount++;
    }

    public void Complete()
    {
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }
}
