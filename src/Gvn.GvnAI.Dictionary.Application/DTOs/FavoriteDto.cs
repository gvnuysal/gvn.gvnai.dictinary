namespace Gvn.GvnAI.Dictionary.Application.DTOs;

public sealed record FavoriteWordDto(
    Guid WordId,
    string Lemma,
    LanguageSummaryDto Language,
    PartOfSpeechSummaryDto PartOfSpeech,
    string? FirstTranslation,
    DateTime AddedAt);
