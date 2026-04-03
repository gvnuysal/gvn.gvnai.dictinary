using Gvn.GvnAI.Dictionary.Application.DTOs;

namespace Gvn.GvnAI.Dictionary.Application.Abstractions;

public interface IAiDictionaryService
{
    Task<WordEnrichmentResult?> EnrichWordAsync(
        string lemma, string languageCode, string targetLanguageCode,
        CancellationToken cancellationToken = default);
}
