using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.CreateWord;

public sealed record CreateWordCommand(
    string Lemma,
    Guid LanguageId,
    Guid PartOfSpeechId,
    Guid UserId) : ICommand<Guid>;
