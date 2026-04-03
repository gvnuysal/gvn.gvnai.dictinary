using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.UpdateWord;

public sealed class UpdateWordCommandHandler(
    IWordRepository wordRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateWordCommand>
{
    public async Task<Result> Handle(UpdateWordCommand request, CancellationToken cancellationToken)
    {
        var word = await wordRepository.GetByIdAsync(request.Id, cancellationToken);
        if (word is null)
            return Result.Fail(DomainErrors.Word.NotFound(request.Id));

        word.Update(
            request.Lemma, request.LanguageId, request.PartOfSpeechId,
            request.FrequencyRank, request.DifficultyLevel,
            request.IsCompound, request.IsIdiom, request.IsProperNoun);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
