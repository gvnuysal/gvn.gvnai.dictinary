namespace Gvn.GvnAI.Dictionary.Application.DTOs;

public sealed record QuizQuestionDto(
    Guid SessionId,
    Guid WordId,
    string Lemma,
    string Definition,
    List<QuizOptionDto> Options,
    Guid CorrectOptionId,
    int QuestionNumber,
    int TotalScore,
    int CorrectCount,
    int WrongCount);

public sealed record QuizOptionDto(
    Guid Id,         // Translation ID
    string Text);    // Turkish translation text

public sealed record QuizAnswerResultDto(
    bool IsCorrect,
    int PointsEarned,
    int TotalScore,
    Guid CorrectOptionId,
    Guid? SelectedOptionId);

public sealed record QuizResultDto(
    Guid SessionId,
    int TotalScore,
    int CorrectCount,
    int WrongCount,
    int TotalQuestions,
    double Accuracy,
    List<QuizAnswerDetailDto> Answers);

public sealed record QuizAnswerDetailDto(
    string WordLemma,
    string CorrectTranslation,
    string? SelectedTranslation,
    bool IsCorrect,
    int PointsEarned,
    int ResponseTimeMs);

public sealed record LeaderboardEntryDto(
    string UserFullName,
    int TotalScore,
    int GamesPlayed,
    double AverageAccuracy);
