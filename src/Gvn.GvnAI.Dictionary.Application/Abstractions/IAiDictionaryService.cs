using Gvn.GvnAI.Dictionary.Application.DTOs;

namespace Gvn.GvnAI.Dictionary.Application.Abstractions;

public interface IAiDictionaryService
{
    Task<WordEnrichmentResult?> EnrichWordAsync(
        string apiKey, string lemma, string languageCode, string targetLanguageCode,
        CancellationToken cancellationToken = default);

    Task<string?> GetDefinitionAsync(string apiKey, string word, CancellationToken cancellationToken = default);

    Task<string?> TranslateWordAsync(string apiKey, string word, string targetLang, CancellationToken cancellationToken = default);

    Task<string?> DetectPartOfSpeechAsync(string apiKey, string word, CancellationToken cancellationToken = default);

    Task<(string? Synonyms, string? Antonyms)> GetSynonymsAsync(string apiKey, string word, CancellationToken cancellationToken = default);
}
