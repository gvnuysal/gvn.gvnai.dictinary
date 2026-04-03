using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Examples.Commands.RemoveExample;

public sealed class RemoveExampleCommandHandler(
    IWordRepository wordRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveExampleCommand>
{
    public async Task<Result> Handle(RemoveExampleCommand request, CancellationToken cancellationToken)
    {
        var word = await wordRepository.GetByIdWithSensesAsync(request.WordId, cancellationToken);
        if (word is null)
            return Result.Fail(DomainErrors.Word.NotFound(request.WordId));

        var sense = word.Senses.FirstOrDefault(s => s.Id == request.SenseId);
        if (sense is null)
            return Result.Fail(DomainErrors.Sense.NotInWord(request.SenseId, request.WordId));

        var example = sense.Examples.FirstOrDefault(e => e.Id == request.ExampleId);
        if (example is null)
            return Result.Fail(DomainErrors.Example.NotFound(request.ExampleId));

        sense.RemoveExample(request.ExampleId);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
