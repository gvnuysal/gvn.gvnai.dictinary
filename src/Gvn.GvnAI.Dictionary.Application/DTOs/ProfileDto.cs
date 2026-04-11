namespace Gvn.GvnAI.Dictionary.Application.DTOs;

public sealed record ProfileDto(
    Guid Id,
    string Email,
    string FullName,
    string Role,
    DateTime MemberSince,
    ProfileStatsDto Stats,
    ApiSettingsDto ApiSettings);

public sealed record ProfileStatsDto(
    int TotalGamesPlayed,
    int TotalScore,
    int TotalCorrect,
    int TotalWrong,
    int TotalQuestionsAnswered,
    double AverageAccuracy,
    int FavoriteWordsCount,
    int BestGameScore);

public sealed record ApiSettingsDto(
    string TranslateProvider,
    bool HasClaudeKey,
    bool HasGoogleKey,
    string? ClaudeApiKey,
    string? GoogleTranslateApiKey,
    bool QuizAutoSpeak);
