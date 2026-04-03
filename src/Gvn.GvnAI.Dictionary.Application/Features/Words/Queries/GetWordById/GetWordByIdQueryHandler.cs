using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnAI.Dictionary.Application.Mappings;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Queries.GetWordById;

public sealed class GetWordByIdQueryHandler(
    IWordRepository wordRepository,
    ILanguageRepository languageRepository,
    IPartOfSpeechRepository partOfSpeechRepository,
    IRegisterRepository registerRepository,
    ISubjectDomainRepository subjectDomainRepository) : IQueryHandler<GetWordByIdQuery, WordDto>
{
    public async Task<Result<WordDto>> Handle(GetWordByIdQuery request, CancellationToken cancellationToken)
    {
        var word = await wordRepository.GetByIdFullAsync(request.Id, cancellationToken);
        if (word is null)
            return Result<WordDto>.Fail(DomainErrors.Word.NotFound(request.Id));

        var allLanguages = await languageRepository.GetAllAsync(cancellationToken);
        var allPos = await partOfSpeechRepository.GetAllAsync(cancellationToken);
        var allRegisters = await registerRepository.GetAllAsync(cancellationToken);
        var allDomains = await subjectDomainRepository.GetAllAsync(cancellationToken);

        var langDict = allLanguages.ToDictionary(l => l.Id, l => l.ToSummaryDto());
        var posDict = allPos.ToDictionary(p => p.Id, p => p.ToSummaryDto());
        var regDict = allRegisters.ToDictionary(r => r.Id, r => (r.Code, r.Name));
        var domDict = allDomains.ToDictionary(d => d.Id, d => (d.Code, d.Name));

        var language = langDict.GetValueOrDefault(word.LanguageId)
                       ?? new LanguageSummaryDto(word.LanguageId, "??", "Unknown");
        var pos = posDict.GetValueOrDefault(word.PartOfSpeechId)
                  ?? new PartOfSpeechSummaryDto(word.PartOfSpeechId, "??", "Unknown");

        var dto = word.ToDto(language, pos, regDict, domDict, langDict, posDict, langDict);

        return Result<WordDto>.Ok(dto);
    }
}
