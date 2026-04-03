using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.CreateWordWithTranslation;

public sealed record CreateWordWithTranslationCommand(
    string Lemma,
    Guid PartOfSpeechId,
    string Definition,
    string TranslationText,
    string? DefinitionShort = null,
    Guid? RegisterId = null,
    Guid? DomainId = null) : ICommand<Guid>;
