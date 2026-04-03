using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.DeleteWord;

public sealed record DeleteWordCommand(Guid Id) : ICommand;
