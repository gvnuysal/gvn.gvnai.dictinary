using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Translations.Commands.AddTranslation;

public sealed class AddTranslationCommandHandler(
    ISenseRepository senseRepository,
    IRepository<Translation> translationRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddTranslationCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddTranslationCommand request, CancellationToken cancellationToken)
    {
        var sense = await senseRepository.GetByIdAsync(request.SenseId, cancellationToken);
        if (sense is null)
            return Result<Guid>.Fail(DomainErrors.Sense.NotFound(request.SenseId));

        if (sense.WordId != request.WordId)
            return Result<Guid>.Fail(DomainErrors.Sense.NotInWord(request.SenseId, request.WordId));

        var translation = Translation.Create(
            request.SenseId, request.TargetLanguageId, request.TranslationText,
            request.PartOfSpeechId, request.RegisterId,
            request.EquivalenceType, request.ConfidenceScore);

        await translationRepository.AddAsync(translation, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Ok(translation.Id);
    }
}
