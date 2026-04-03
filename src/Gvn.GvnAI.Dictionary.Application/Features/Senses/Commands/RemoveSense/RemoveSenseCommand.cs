using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Senses.Commands.RemoveSense;

public sealed record RemoveSenseCommand(Guid WordId, Guid SenseId) : ICommand;
