namespace Gvn.GvnAI.Dictionary.Application.DTOs;

public sealed record PronunciationDto(
    Guid Id, string IpaTranscription, string? Variant, bool IsStandard);
