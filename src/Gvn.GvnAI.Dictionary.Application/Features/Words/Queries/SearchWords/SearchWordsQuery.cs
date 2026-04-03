using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnAI.Dictionary.Application.Features.Words.Queries.GetWords;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Queries.SearchWords;

public sealed record SearchWordsQuery(
    string? Query,
    Guid? LanguageId,
    Guid? PartOfSpeechId,
    Guid? DomainId,
    Guid? RegisterId,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedResult<WordSummaryDto>>;
