using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;

namespace Gvn.GvnAI.Dictionary.Application.DTOs;

public sealed record WordDto(
    Guid Id, string Lemma,
    LanguageSummaryDto Language, PartOfSpeechSummaryDto PartOfSpeech,
    WordStatus Status, int? FrequencyRank, DifficultyLevel? DifficultyLevel,
    bool IsCompound, bool IsIdiom, bool IsProperNoun,
    string? Synonyms, string? Antonyms,
    List<SenseDto> Senses,
    List<PronunciationDto> Pronunciations,
    List<EtymologyDto> Etymologies,
    DateTime CreatedAt, DateTime? UpdatedAt);

public sealed record WordSummaryDto(
    Guid Id, string Lemma,
    LanguageSummaryDto Language, PartOfSpeechSummaryDto PartOfSpeech,
    WordStatus Status, DateTime CreatedAt,
    string? FirstDefinition, string? FirstTranslation);
