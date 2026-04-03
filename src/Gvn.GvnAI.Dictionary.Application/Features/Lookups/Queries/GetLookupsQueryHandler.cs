using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnAI.Dictionary.Application.Mappings;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;

namespace Gvn.GvnAI.Dictionary.Application.Features.Lookups.Queries;

public sealed class GetLookupsQueryHandler(
    ILanguageRepository languageRepository,
    IPartOfSpeechRepository partOfSpeechRepository,
    IRegisterRepository registerRepository,
    ISubjectDomainRepository subjectDomainRepository) : IQueryHandler<GetLookupsQuery, LookupsDto>
{
    public async Task<Result<LookupsDto>> Handle(GetLookupsQuery request, CancellationToken cancellationToken)
    {
        var languages = await languageRepository.GetAllAsync(cancellationToken);
        var partsOfSpeech = await partOfSpeechRepository.GetAllAsync(cancellationToken);
        var registers = await registerRepository.GetAllAsync(cancellationToken);
        var domains = await subjectDomainRepository.GetAllAsync(cancellationToken);

        var dto = new LookupsDto(
            languages.Select(l => l.ToDto()).ToList(),
            partsOfSpeech.Select(p => p.ToDto()).ToList(),
            registers.Select(r => r.ToDto()).ToList(),
            domains.Select(d => d.ToDto()).ToList());

        return Result<LookupsDto>.Ok(dto);
    }
}
