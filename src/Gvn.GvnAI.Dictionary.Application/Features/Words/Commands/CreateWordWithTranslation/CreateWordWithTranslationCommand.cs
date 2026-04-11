using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Words.Commands.CreateWordWithTranslation;

public sealed record CreateWordWithTranslationCommand(
    string Lemma,
    Guid PartOfSpeechId,
    Guid UserId,
    string Definition,
    string TranslationText,
    string? DefinitionShort = null,
    Guid? RegisterId = null,
    Guid? DomainId = null,
    string? Synonyms = null,
    string? Antonyms = null) : ICommand<Guid>;
