using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Examples.Commands.AddExample;

public sealed record AddExampleCommand(
    Guid WordId, Guid SenseId, Guid? TranslationId,
    string SourceText, string? TargetText, ExampleSource Source) : ICommand<Guid>;
