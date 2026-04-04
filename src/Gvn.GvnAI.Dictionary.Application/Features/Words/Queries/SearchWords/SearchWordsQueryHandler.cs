using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnAI.Dictionary.Application.Features.Words.Queries.GetWords;
using Gvn.GvnAI.Dictionary.Application.Mappings;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Queries.SearchWords;

public sealed class SearchWordsQueryHandler(
    IWordRepository wordRepository,
    ILanguageRepository languageRepository,
    IPartOfSpeechRepository partOfSpeechRepository) : IQueryHandler<SearchWordsQuery, PagedResult<WordSummaryDto>>
{
    public async Task<Result<PagedResult<WordSummaryDto>>> Handle(
        SearchWordsQuery request, CancellationToken cancellationToken)
    {
        var skip = (request.PageNumber - 1) * request.PageSize;
        var (items, totalCount) = await wordRepository.SearchAsync(
            request.Query, request.LanguageId, request.PartOfSpeechId,
            request.DomainId, request.RegisterId,
            skip, request.PageSize, request.UserId, cancellationToken);

        var allLanguages = await languageRepository.GetAllAsync(cancellationToken);
        var allPos = await partOfSpeechRepository.GetAllAsync(cancellationToken);

        var langDict = allLanguages.ToDictionary(l => l.Id, l => l.ToSummaryDto());
        var posDict = allPos.ToDictionary(p => p.Id, p => p.ToSummaryDto());

        var dtos = items.Select(w =>
        {
            var lang = langDict.GetValueOrDefault(w.LanguageId)
                       ?? new LanguageSummaryDto(w.LanguageId, "??", "Unknown");
            var pos = posDict.GetValueOrDefault(w.PartOfSpeechId)
                      ?? new PartOfSpeechSummaryDto(w.PartOfSpeechId, "??", "Unknown");
            return w.ToSummaryDto(lang, pos);
        }).ToList();

        var result = new PagedResult<WordSummaryDto>(dtos, totalCount, request.PageNumber, request.PageSize);

        return Result<PagedResult<WordSummaryDto>>.Ok(result);
    }
}
