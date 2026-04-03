using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Senses.Commands.UpdateSense;

public sealed record UpdateSenseCommand(
    Guid WordId, Guid SenseId, string Definition, string? DefinitionShort,
    Guid? RegisterId, Guid? DomainId,
    int? FrequencyRank, DifficultyLevel? DifficultyLevel) : ICommand;
