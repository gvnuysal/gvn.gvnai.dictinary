namespace Gvn.GvnAI.Dictionary.Application.DTOs;

public sealed record EtymologyDto(
    Guid Id, LanguageSummaryDto? OriginLanguage, string Text);
