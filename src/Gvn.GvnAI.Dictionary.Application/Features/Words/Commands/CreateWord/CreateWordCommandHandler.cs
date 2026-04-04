using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.CreateWord;

public sealed class CreateWordCommandHandler(
    IWordRepository wordRepository,
    ILanguageRepository languageRepository,
    IPartOfSpeechRepository partOfSpeechRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateWordCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateWordCommand request, CancellationToken cancellationToken)
    {
        var language = await languageRepository.GetByIdAsync(request.LanguageId, cancellationToken);
        if (language is null)
            return Result<Guid>.Fail(DomainErrors.Language.NotFound(request.LanguageId));

        var pos = await partOfSpeechRepository.GetByIdAsync(request.PartOfSpeechId, cancellationToken);
        if (pos is null)
            return Result<Guid>.Fail(DomainErrors.PartOfSpeech.NotFound(request.PartOfSpeechId));

        var existing = await wordRepository.GetByLemmaAsync(request.Lemma, request.LanguageId, request.UserId, cancellationToken);
        if (existing is not null)
            return Result<Guid>.Fail(DomainErrors.Word.DuplicateLemma(request.Lemma));

        var word = Word.Create(request.Lemma, request.LanguageId, request.PartOfSpeechId, request.UserId);

        await wordRepository.AddAsync(word, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Ok(word.Id);
    }
}
