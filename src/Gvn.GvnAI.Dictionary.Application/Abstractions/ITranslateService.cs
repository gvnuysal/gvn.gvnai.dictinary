namespace Gvn.GvnAI.Dictionary.Application.Abstractions;

public interface ITranslateService
{
    Task<string?> TranslateAsync(string apiKey, string text, string sourceLang, string targetLang, CancellationToken ct = default);
}
