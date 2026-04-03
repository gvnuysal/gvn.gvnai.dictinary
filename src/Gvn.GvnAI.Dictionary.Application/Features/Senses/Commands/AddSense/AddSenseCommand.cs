using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Senses.Commands.AddSense;

public sealed record AddSenseCommand(
    Guid WordId, string Definition, string? DefinitionShort,
    Guid? RegisterId, Guid? DomainId,
    int? FrequencyRank, DifficultyLevel? DifficultyLevel) : ICommand<Guid>;
