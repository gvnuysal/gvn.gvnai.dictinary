using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.BackgroundJobs.Abstractions;
using Gvn.GvnFramework.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Jobs;

public class EnrichPendingWordsJob(
    IWordRepository wordRepository,
    ILanguageRepository languageRepository,
    IPartOfSpeechRepository partOfSpeechRepository,
    IRegisterRepository registerRepository,
    ISubjectDomainRepository subjectDomainRepository,
    IAiDictionaryService aiService,
    IUnitOfWork unitOfWork,
    ILogger<EnrichPendingWordsJob> logger) : IRecurringJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var pendingWords = await wordRepository.GetPendingWordsAsync(10, cancellationToken);
        if (!pendingWords.Any()) return;

        var allLanguages = await languageRepository.GetAllAsync(cancellationToken);
        var allPos = await partOfSpeechRepository.GetAllAsync(cancellationToken);
        var allRegisters = await registerRepository.GetAllAsync(cancellationToken);
        var allDomains = await subjectDomainRepository.GetAllAsync(cancellationToken);

        var langById = allLanguages.ToDictionary(l => l.Id);
        var langByCode = allLanguages.ToDictionary(l => l.Code, StringComparer.OrdinalIgnoreCase);
        var posLookup = allPos.ToDictionary(p => p.Code, StringComparer.OrdinalIgnoreCase);
        var regLookup = allRegisters.ToDictionary(r => r.Code, StringComparer.OrdinalIgnoreCase);
        var domLookup = allDomains.ToDictionary(d => d.Code, StringComparer.OrdinalIgnoreCase);

        foreach (var word in pendingWords)
        {
            try
            {
                if (!langById.TryGetValue(word.LanguageId, out var language))
                {
                    logger.LogWarning("Language not found for word '{Lemma}', skipping", word.Lemma);
                    word.MarkFailed();
                    continue;
                }

                var targetCode = language.Code == "tr" ? "en" : "tr";

                var enrichment = await aiService.EnrichWordAsync(
                    word.Lemma, language.Code, targetCode, cancellationToken);

                if (enrichment is null)
                {
                    word.MarkFailed();
                    continue;
                }

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
                        var targetLang = langByCode.GetValueOrDefault(et.TargetLanguageCode ?? targetCode);
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
                logger.LogInformation("Enriched word '{Lemma}' successfully", word.Lemma);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to enrich word '{Lemma}'", word.Lemma);
                word.MarkFailed();
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
