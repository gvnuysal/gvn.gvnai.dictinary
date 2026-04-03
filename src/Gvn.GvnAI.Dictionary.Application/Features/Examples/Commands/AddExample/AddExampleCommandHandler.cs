using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Examples.Commands.AddExample;

public sealed class AddExampleCommandHandler(
    ISenseRepository senseRepository,
    IRepository<Example> exampleRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddExampleCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddExampleCommand request, CancellationToken cancellationToken)
    {
        var sense = await senseRepository.GetByIdAsync(request.SenseId, cancellationToken);
        if (sense is null)
            return Result<Guid>.Fail(DomainErrors.Sense.NotFound(request.SenseId));

        if (sense.WordId != request.WordId)
            return Result<Guid>.Fail(DomainErrors.Sense.NotInWord(request.SenseId, request.WordId));

        var example = Example.Create(
            request.SenseId, request.TranslationId,
            request.SourceText, request.TargetText, request.Source);

        await exampleRepository.AddAsync(example, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Ok(example.Id);
    }
}
