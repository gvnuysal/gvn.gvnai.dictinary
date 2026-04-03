using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Senses.Commands.UpdateSense;

public sealed class UpdateSenseCommandHandler(
    IWordRepository wordRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateSenseCommand>
{
    public async Task<Result> Handle(UpdateSenseCommand request, CancellationToken cancellationToken)
    {
        var word = await wordRepository.GetByIdWithSensesAsync(request.WordId, cancellationToken);
        if (word is null)
            return Result.Fail(DomainErrors.Word.NotFound(request.WordId));

        var sense = word.Senses.FirstOrDefault(s => s.Id == request.SenseId);
        if (sense is null)
            return Result.Fail(DomainErrors.Sense.NotInWord(request.SenseId, request.WordId));

        sense.Update(
            request.Definition, request.DefinitionShort,
            request.RegisterId, request.DomainId,
            request.FrequencyRank, request.DifficultyLevel);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
