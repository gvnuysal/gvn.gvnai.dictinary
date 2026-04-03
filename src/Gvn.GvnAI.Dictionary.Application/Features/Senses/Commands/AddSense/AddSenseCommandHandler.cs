using Gvn.GvnAI.Dictionary.Domain.Entities;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Gvn.GvnAI.Dictionary.Domain.Shared.Errors;
using Gvn.GvnFramework.Application.Abstractions;
using Gvn.GvnFramework.Core.Results;
using Gvn.GvnFramework.Domain.Repositories;

namespace Gvn.GvnAI.Dictionary.Application.Features.Senses.Commands.AddSense;

public sealed class AddSenseCommandHandler(
    IWordRepository wordRepository,
    ISenseRepository senseRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddSenseCommand, Guid>
{
    public async Task<Result<Guid>> Handle(AddSenseCommand request, CancellationToken cancellationToken)
    {
        var exists = await wordRepository.ExistsAsync(request.WordId, cancellationToken);
        if (!exists)
            return Result<Guid>.Fail(DomainErrors.Word.NotFound(request.WordId));

        var maxSenseNumber = await senseRepository.GetMaxSenseNumberAsync(request.WordId, cancellationToken);
        var sense = Sense.Create(
            request.Definition, request.DefinitionShort,
            maxSenseNumber + 1, request.WordId,
            request.RegisterId, request.DomainId,
            request.FrequencyRank, request.DifficultyLevel);

        await senseRepository.AddAsync(sense, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Ok(sense.Id);
    }
}
