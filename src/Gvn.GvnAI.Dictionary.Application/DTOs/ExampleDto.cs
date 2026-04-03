using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;

namespace Gvn.GvnAI.Dictionary.Application.DTOs;

public sealed record ExampleDto(
    Guid Id, string SourceText, string? TargetText, ExampleSource Source);
