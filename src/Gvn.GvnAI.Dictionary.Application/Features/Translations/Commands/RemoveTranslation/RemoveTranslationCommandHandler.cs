using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Translations.Commands.RemoveTranslation;

public sealed class RemoveTranslationCommandHandler(
    IWordRepository wordRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveTranslationCommand>
{
    public async Task<Result> Handle(RemoveTranslationCommand request, CancellationToken cancellationToken)
    {
        var word = await wordRepository.GetByIdWithSensesAsync(request.WordId, cancellationToken);
        if (word is null)
            return Result.Fail(DomainErrors.Word.NotFound(request.WordId));

        var sense = word.Senses.FirstOrDefault(s => s.Id == request.SenseId);
        if (sense is null)
            return Result.Fail(DomainErrors.Sense.NotInWord(request.SenseId, request.WordId));

        var translation = sense.Translations.FirstOrDefault(t => t.Id == request.TranslationId);
        if (translation is null)
            return Result.Fail(DomainErrors.Translation.NotFound(request.TranslationId));

        sense.RemoveTranslation(request.TranslationId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
