namespace Gvn.GvnAI.Dictionary.Application.DTOs;

public sealed record WordEnrichmentResult(
    List<EnrichmentSense> Senses,
    List<EnrichmentPronunciation> Pronunciations,
    string? Etymology);

public sealed record EnrichmentSense(
    string Definition, string? DefinitionShort,
    string? RegisterCode, string? DomainCode,
    List<EnrichmentTranslation> Translations,
    List<EnrichmentExample> Examples);

public sealed record EnrichmentTranslation(
    string Text, string TargetLanguageCode,
    string? PartOfSpeechCode,
    string EquivalenceType, double ConfidenceScore);

public sealed record EnrichmentExample(
    string SourceText, string? TargetText);

public sealed record EnrichmentPronunciation(
    string Ipa, string? Variant, bool IsStandard);
