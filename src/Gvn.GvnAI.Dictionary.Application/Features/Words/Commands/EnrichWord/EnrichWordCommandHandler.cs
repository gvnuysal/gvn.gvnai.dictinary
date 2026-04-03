using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.EnrichWord;

public sealed class EnrichWordCommandHandler(
    IWordRepository wordRepository,
    ILanguageRepository languageRepository,
    IPartOfSpeechRepository partOfSpeechRepository,
    IRegisterRepository registerRepository,
    ISubjectDomainRepository subjectDomainRepository,
    IAiDictionaryService aiService,
    IUnitOfWork unitOfWork) : ICommandHandler<EnrichWordCommand>
{
    public async Task<Result> Handle(EnrichWordCommand request, CancellationToken cancellationToken)
    {
        var word = await wordRepository.GetByIdWithSensesAsync(request.WordId, cancellationToken);
        if (word is null)
            return Result.Fail(DomainErrors.Word.NotFound(request.WordId));

        var language = await languageRepository.GetByIdAsync(word.LanguageId, cancellationToken);
        if (language is null)
            return Result.Fail(DomainErrors.Language.NotFound(word.LanguageId));

        var targetCode = request.TargetLanguageCode
                         ?? (language.Code == "tr" ? "en" : "tr");

        var enrichment = await aiService.EnrichWordAsync(
            word.Lemma, language.Code, targetCode, cancellationToken);

        if (enrichment is null)
            return Result.Fail(DomainErrors.Word.EnrichmentFailed(word.Lemma));

        var allPos = await partOfSpeechRepository.GetAllAsync(cancellationToken);
        var allRegisters = await registerRepository.GetAllAsync(cancellationToken);
        var allDomains = await subjectDomainRepository.GetAllAsync(cancellationToken);
        var allLanguages = await languageRepository.GetAllAsync(cancellationToken);

        var posLookup = allPos.ToDictionary(p => p.Code, StringComparer.OrdinalIgnoreCase);
        var regLookup = allRegisters.ToDictionary(r => r.Code, StringComparer.OrdinalIgnoreCase);
        var domLookup = allDomains.ToDictionary(d => d.Code, StringComparer.OrdinalIgnoreCase);
        var langLookup = allLanguages.ToDictionary(l => l.Code, StringComparer.OrdinalIgnoreCase);

        var senses = new List<Sense>();
        var senseNumber = 1;

        foreach (var es in enrichment.Senses)
        {
            Guid? registerId = es.RegisterCode is not null && regLookup.TryGetValue(es.RegisterCode, out var reg)
                ? reg.Id : null;
            Guid? domainId = es.DomainCode is not null && domLookup.TryGetValue(es.DomainCode, out var dom)
                ? dom.Id : null;

            var sense = Sense.Create(
                es.Definition, es.DefinitionShort, senseNumber++, word.Id,
                registerId, domainId);

            foreach (var et in es.Translations)
            {
                var targetLang = langLookup.GetValueOrDefault(et.TargetLanguageCode ?? targetCode);
                if (targetLang is null) continue;

                Guid? posId = et.PartOfSpeechCode is not null && posLookup.TryGetValue(et.PartOfSpeechCode, out var pos)
                    ? pos.Id : null;

                var eqType = Enum.TryParse<EquivalenceType>(et.EquivalenceType, true, out var eq)
                    ? eq : EquivalenceType.Near;

                sense.AddTranslation(targetLang.Id, et.Text, posId, null, eqType, et.ConfidenceScore);
            }

            foreach (var ex in es.Examples)
            {
                sense.AddExample(ex.SourceText, ex.TargetText, ExampleSource.Ai);
            }

            senses.Add(sense);
        }

        var pronunciations = enrichment.Pronunciations
            .Select(p => Pronunciation.Create(p.Ipa, p.Variant, p.IsStandard, word.Id))
            .ToList();

        Etymology? etymology = enrichment.Etymology is not null
            ? Etymology.Create(null, enrichment.Etymology, word.Id)
            : null;

        word.Enrich(senses, pronunciations, etymology);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
