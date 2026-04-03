using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Senses.Commands.RemoveSense;

public sealed class RemoveSenseCommandHandler(
    IWordRepository wordRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveSenseCommand>
{
    public async Task<Result> Handle(RemoveSenseCommand request, CancellationToken cancellationToken)
    {
        var word = await wordRepository.GetByIdWithSensesAsync(request.WordId, cancellationToken);
        if (word is null)
            return Result.Fail(DomainErrors.Word.NotFound(request.WordId));

        var sense = word.Senses.FirstOrDefault(s => s.Id == request.SenseId);
        if (sense is null)
            return Result.Fail(DomainErrors.Sense.NotInWord(request.SenseId, request.WordId));

        word.RemoveSense(request.SenseId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
