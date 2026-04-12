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
    ISenseRepository senseRepository,
    ILanguageRepository languageRepository,
    IPartOfSpeechRepository partOfSpeechRepository,
    IRegisterRepository registerRepository,
    ISubjectDomainRepository subjectDomainRepository,
    IRepository<Translation> translationRepository,
    IRepository<Example> exampleRepository,
    IRepository<Pronunciation> pronunciationRepository,
    IRepository<Etymology> etymologyRepository,
    IAiDictionaryService aiService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<EnrichWordCommand>
{
    public async Task<Result> Handle(EnrichWordCommand request, CancellationToken cancellationToken)
    {
        var word = await wordRepository.GetByIdAsync(request.WordId, cancellationToken);
        if (word is null)
            return Result.Fail(DomainErrors.Word.NotFound(request.WordId));

        var language = await languageRepository.GetByIdAsync(word.LanguageId, cancellationToken);
        if (language is null)
            return Result.Fail(DomainErrors.Language.NotFound(word.LanguageId));

        var targetCode = request.TargetLanguageCode
                         ?? (language.Code == "tr" ? "en" : "tr");

        // Kullanıcının Claude API key'ini DB'den al
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        var claudeKey = user?.ClaudeApiKey;
        if (string.IsNullOrWhiteSpace(claudeKey))
            return Result.Fail(Error.Validation("API_KEY_MISSING", "Claude API key ayarlanmamis. Profil sayfasindan ekleyin."));

        var enrichment = await aiService.EnrichWordAsync(
            claudeKey, word.Lemma, language.Code, targetCode, cancellationToken);

        if (enrichment is null)
        {
            word.MarkFailed();
            await wordRepository.UpdateAsync(word, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Fail(DomainErrors.Word.EnrichmentFailed(word.Lemma));
        }

        // Mevcut sense'leri sil (cascade ile translations, examples da silinir)
        await senseRepository.DeleteAllByWordIdAsync(word.Id, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Lookup tablolarını yükle
        var allPos = await partOfSpeechRepository.GetAllAsync(cancellationToken);
        var allRegisters = await registerRepository.GetAllAsync(cancellationToken);
        var allDomains = await subjectDomainRepository.GetAllAsync(cancellationToken);
        var allLanguages = await languageRepository.GetAllAsync(cancellationToken);

        var posLookup = allPos.ToDictionary(p => p.Code, StringComparer.OrdinalIgnoreCase);
        var regLookup = allRegisters.ToDictionary(r => r.Code, StringComparer.OrdinalIgnoreCase);
        var domLookup = allDomains.ToDictionary(d => d.Code, StringComparer.OrdinalIgnoreCase);
        var langLookup = allLanguages.ToDictionary(l => l.Code, StringComparer.OrdinalIgnoreCase);

        // Her sense, translation, example'ı doğrudan repository ile ekle
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
            await senseRepository.AddAsync(sense, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            foreach (var et in es.Translations)
            {
                var targetLang = langLookup.GetValueOrDefault(et.TargetLanguageCode ?? targetCode);
                if (targetLang is null) continue;

                Guid? posId = et.PartOfSpeechCode is not null && posLookup.TryGetValue(et.PartOfSpeechCode, out var pos)
                    ? pos.Id : null;

                var eqType = Enum.TryParse<EquivalenceType>(et.EquivalenceType, true, out var eq)
                    ? eq : EquivalenceType.Near;

                var translation = Translation.Create(sense.Id, targetLang.Id, et.Text, posId, null, eqType, et.ConfidenceScore);
                await translationRepository.AddAsync(translation, cancellationToken);
            }

            foreach (var ex in es.Examples)
            {
                var example = Example.Create(sense.Id, null, ex.SourceText, ex.TargetText, ExampleSource.Ai);
                await exampleRepository.AddAsync(example, cancellationToken);
            }

            await unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Telaffuz
        foreach (var p in enrichment.Pronunciations)
        {
            var pron = Pronunciation.Create(p.Ipa, p.Variant, p.IsStandard, word.Id);
            await pronunciationRepository.AddAsync(pron, cancellationToken);
        }

        // Köken
        if (enrichment.Etymology is not null)
        {
            var etym = Etymology.Create(null, enrichment.Etymology, word.Id);
            await etymologyRepository.AddAsync(etym, cancellationToken);
        }

        // Synonyms / antonyms (best effort — başarısız olursa zenginleştirmeyi blocklamayalım)
        try
        {
            var (synonyms, antonyms) = await aiService.GetSynonymsAsync(claudeKey, word.Lemma, cancellationToken);
            if (!string.IsNullOrWhiteSpace(synonyms) || !string.IsNullOrWhiteSpace(antonyms))
                word.UpdateSynonyms(synonyms, antonyms);
        }
        catch
        {
            // ignore — synonyms zenginleştirme akışını bozmamalı
        }

        // Status güncelle
        word.Approve();
        await wordRepository.UpdateAsync(word, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
