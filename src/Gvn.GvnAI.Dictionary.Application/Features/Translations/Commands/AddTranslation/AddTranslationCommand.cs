using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Translations.Commands.AddTranslation;

public sealed record AddTranslationCommand(
    Guid WordId, Guid SenseId, Guid TargetLanguageId,
    string TranslationText, Guid? PartOfSpeechId, Guid? RegisterId,
    EquivalenceType EquivalenceType, double ConfidenceScore) : ICommand<Guid>;
