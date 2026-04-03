using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;

namespace Gvn.GvnAI.Dictionary.Application.DTOs;

public sealed record TranslationDto(
    Guid Id,
    LanguageSummaryDto TargetLanguage,
    string TranslationText,
    PartOfSpeechSummaryDto? PartOfSpeech,
    string? RegisterCode,
    EquivalenceType EquivalenceType,
    double ConfidenceScore);
