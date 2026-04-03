using Gvn.GvnAI.Dictionary.Application.DTOs;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Lookups.Queries;

public sealed record GetLookupsQuery() : IQuery<LookupsDto>;

public sealed record LookupsDto(
    List<LanguageDto> Languages,
    List<PartOfSpeechDto> PartsOfSpeech,
    List<RegisterDto> Registers,
    List<SubjectDomainDto> Domains);
