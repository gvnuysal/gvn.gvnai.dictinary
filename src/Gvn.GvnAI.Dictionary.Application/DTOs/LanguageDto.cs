using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;

namespace Gvn.GvnAI.Dictionary.Application.DTOs;

public sealed record LanguageDto(
    Guid Id, string Code, string Name, string NativeName, TextDirection Direction);
