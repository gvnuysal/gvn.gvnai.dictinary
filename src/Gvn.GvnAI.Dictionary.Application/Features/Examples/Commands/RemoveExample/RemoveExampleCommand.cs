using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Examples.Commands.RemoveExample;

public sealed record RemoveExampleCommand(Guid WordId, Guid SenseId, Guid ExampleId) : ICommand;
