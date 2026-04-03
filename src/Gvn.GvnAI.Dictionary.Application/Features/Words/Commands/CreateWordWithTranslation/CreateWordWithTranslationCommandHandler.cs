using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.CreateWordWithTranslation;

public sealed class CreateWordWithTranslationCommandHandler(
    IWordRepository wordRepository,
    ISenseRepository senseRepository,
    IRepository<Translation> translationRepository,
    ILanguageRepository languageRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateWordWithTranslationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateWordWithTranslationCommand request, CancellationToken cancellationToken)
    {
        // EN ve TR dil ID'lerini al
        var enLang = await languageRepository.GetByCodeAsync("en", cancellationToken);
        var trLang = await languageRepository.GetByCodeAsync("tr", cancellationToken);

        if (enLang is null || trLang is null)
            return Result<Guid>.Fail(Error.Failure("LANG_NOT_FOUND", "English or Turkish language not found in database."));

        // Duplicate kontrolü
        var existing = await wordRepository.GetByLemmaAsync(request.Lemma, enLang.Id, cancellationToken);
        if (existing is not null)
            return Result<Guid>.Fail(DomainErrors.Word.DuplicateLemma(request.Lemma));

        // 1. Word oluştur (İngilizce)
        var word = Word.Create(request.Lemma, enLang.Id, request.PartOfSpeechId);
        await wordRepository.AddAsync(word, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 2. Sense oluştur (İngilizce tanım)
        var sense = Sense.Create(
            request.Definition,
            request.DefinitionShort,
            senseNumber: 1,
            wordId: word.Id,
            registerId: request.RegisterId,
            domainId: request.DomainId);
        await senseRepository.AddAsync(sense, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // 3. Translation oluştur (Türkçe karşılık)
        var translation = Translation.Create(
            senseId: sense.Id,
            targetLanguageId: trLang.Id,
            translationText: request.TranslationText,
            partOfSpeechId: request.PartOfSpeechId,
            registerId: null,
            equivalenceType: EquivalenceType.Exact,
            confidenceScore: 1.0);
        await translationRepository.AddAsync(translation, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Ok(word.Id);
    }
}
