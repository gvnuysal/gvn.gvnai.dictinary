using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Queries.GetWords;

public sealed record GetWordsQuery(
    Guid? LanguageId,
    int PageNumber = 1,
    int PageSize = 10) : IQuery<PagedResult<WordSummaryDto>>;

public sealed record PagedResult<T>(
    List<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize);
