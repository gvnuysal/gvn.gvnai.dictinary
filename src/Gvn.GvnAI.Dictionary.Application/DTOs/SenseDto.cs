using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;

namespace Gvn.GvnAI.Dictionary.Application.DTOs;

public sealed record SenseDto(
    Guid Id, int SenseNumber,
    string Definition, string? DefinitionShort,
    string? RegisterCode, string? RegisterName,
    string? DomainCode, string? DomainName,
    int? FrequencyRank, DifficultyLevel? DifficultyLevel,
    List<TranslationDto> Translations,
    List<ExampleDto> Examples);
