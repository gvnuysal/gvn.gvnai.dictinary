using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.DeleteWord;

public sealed class DeleteWordCommandHandler(
    IWordRepository wordRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteWordCommand>
{
    public async Task<Result> Handle(DeleteWordCommand request, CancellationToken cancellationToken)
    {
        var word = await wordRepository.GetByIdAsync(request.Id, cancellationToken);
        if (word is null)
            return Result.Fail(DomainErrors.Word.NotFound(request.Id));

        await wordRepository.DeleteAsync(word, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
