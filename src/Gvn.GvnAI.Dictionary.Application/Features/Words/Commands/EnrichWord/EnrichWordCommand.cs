using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.EnrichWord;

public sealed record EnrichWordCommand(Guid WordId, string? TargetLanguageCode = null) : ICommand;
