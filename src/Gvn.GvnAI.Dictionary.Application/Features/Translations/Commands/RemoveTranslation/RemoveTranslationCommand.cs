using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Translations.Commands.RemoveTranslation;

public sealed record RemoveTranslationCommand(Guid WordId, Guid SenseId, Guid TranslationId) : ICommand;
