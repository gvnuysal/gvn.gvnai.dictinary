using Gvn.GvnFramework.Application.Abstractions;

namespace Gvn.GvnAI.Dictionary.Application.Features.Profile.Commands.UpdateApiSettings;

public sealed record UpdateApiSettingsCommand(
    Guid UserId,
    string TranslateProvider,
    string? ClaudeApiKey,
    string? GoogleTranslateApiKey) : ICommand;
