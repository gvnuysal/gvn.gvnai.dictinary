using System.Text.Json;
using Anthropic.SDK;
using Anthropic.SDK.Messaging;
using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Application.DTOs;
using Microsoft.Extensions.Logging;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Services;

public class ClaudeAiDictionaryService(
    ILogger<ClaudeAiDictionaryService> logger) : IAiDictionaryService
{
    private const string DefaultModel = "claude-sonnet-4-20250514";
    private const int MaxRetries = 3;

    // Retry helper: 3 deneme, artan bekleme (1s, 2s, 4s)
    private async Task<T?> WithRetryAsync<T>(string apiKey, Func<AnthropicClient, Task<T?>> action, string operationName, CancellationToken ct) where T : class
    {
        if (string.IsNullOrWhiteSpace(apiKey)) { logger.LogWarning("{Op}: API key not provided", operationName); return null; }

        for (var attempt = 1; attempt <= MaxRetries; attempt++)
        {
            try
            {
                var client = new AnthropicClient(apiKey);
                var result = await action(client);
                if (result is not null) return result;
            }
            catch (Exception ex) when (attempt < MaxRetries && !ct.IsCancellationRequested)
            {
                var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt - 1)); // 1s, 2s, 4s
                logger.LogWarning(ex, "{Op}: attempt {Attempt}/{Max} failed, retrying in {Delay}s", operationName, attempt, MaxRetries, delay.TotalSeconds);
                await Task.Delay(delay, ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Op}: all {Max} attempts failed", operationName, MaxRetries);
                return null;
            }
        }
        return null;
    }

    // String donen metotlar icin overload
    private async Task<string?> WithRetryStringAsync(string apiKey, Func<AnthropicClient, Task<string?>> action, string operationName, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(apiKey)) { logger.LogWarning("{Op}: API key not provided", operationName); return null; }

        for (var attempt = 1; attempt <= MaxRetries; attempt++)
        {
            try
            {
                var client = new AnthropicClient(apiKey);
                var result = await action(client);
                if (result is not null) return result;
            }
            catch (Exception ex) when (attempt < MaxRetries && !ct.IsCancellationRequested)
            {
                var delay = TimeSpan.FromSeconds(Math.Pow(2, attempt - 1));
                logger.LogWarning(ex, "{Op}: attempt {Attempt}/{Max} failed, retrying in {Delay}s", operationName, attempt, MaxRetries, delay.TotalSeconds);
                await Task.Delay(delay, ct);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Op}: all {Max} attempts failed", operationName, MaxRetries);
                return null;
            }
        }
        return null;
    }

    private async Task<string?> CallClaude(AnthropicClient client, string prompt, int maxTokens, CancellationToken ct)
    {
        var response = await client.Messages.GetClaudeMessageAsync(new MessageParameters
        {
            Model = DefaultModel, MaxTokens = maxTokens,
            Messages = [new Message(RoleType.User, prompt)]
        }, ctx: ct);
        return response.Content.OfType<TextContent>().FirstOrDefault()?.Text?.Trim();
    }

    public Task<WordEnrichmentResult?> EnrichWordAsync(
        string apiKey, string lemma, string languageCode, string targetLanguageCode,
        CancellationToken cancellationToken = default)
    {
        return WithRetryAsync<WordEnrichmentResult>(apiKey, async client =>
        {
            var prompt = $$"""
                You are a bilingual dictionary expert. Analyze the word "{{lemma}}" in {{languageCode}}.
                Translate meanings to {{targetLanguageCode}}.
                Return ONLY valid JSON (no markdown):
                {"senses":[{"definition":"...","definitionShort":"...","registerCode":null,"domainCode":null,
                  "translations":[{"text":"...","targetLanguageCode":"{{targetLanguageCode}}","partOfSpeechCode":null,"equivalenceType":"Exact","confidenceScore":0.95}],
                  "examples":[{"sourceText":"...","targetText":"..."}]}],
                "pronunciations":[{"ipa":"/.../ ","variant":null,"isStandard":true}],"etymology":"..."}
                Provide 1-5 senses. ConfidenceScore 0.0-1.0.
                """;
            var text = await CallClaude(client, prompt, 2048, cancellationToken);
            if (string.IsNullOrWhiteSpace(text)) return null;
            return JsonSerializer.Deserialize<WordEnrichmentResult>(text, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }, $"Enrich({lemma})", cancellationToken);
    }

    public Task<string?> GetDefinitionAsync(string apiKey, string word, CancellationToken cancellationToken = default)
    {
        return WithRetryStringAsync(apiKey, async client =>
            await CallClaude(client, $"Define the English word \"{word}\" in one clear, concise sentence. Return ONLY the definition text.", 150, cancellationToken),
            $"Define({word})", cancellationToken);
    }

    public Task<string?> TranslateWordAsync(string apiKey, string word, string targetLang, CancellationToken cancellationToken = default)
    {
        var langName = targetLang == "tr" ? "Turkish" : targetLang;
        return WithRetryStringAsync(apiKey, async client =>
            await CallClaude(client, $"Translate the English word \"{word}\" to {langName}. Return ONLY the translated word(s). Max 3, comma separated.", 50, cancellationToken),
            $"Translate({word}→{targetLang})", cancellationToken);
    }

    public Task<string?> DetectPartOfSpeechAsync(string apiKey, string word, CancellationToken cancellationToken = default)
    {
        return WithRetryStringAsync(apiKey, async client =>
        {
            var result = await CallClaude(client, $"What is the primary part of speech of \"{word}\"? Reply ONLY: NOUN, VERB, ADJ, ADV, PRON, PREP, CONJ, INTERJ", 10, cancellationToken);
            return result?.ToUpperInvariant();
        }, $"DetectPOS({word})", cancellationToken);
    }

    public async Task<(string? Synonyms, string? Antonyms)> GetSynonymsAsync(string apiKey, string word, CancellationToken cancellationToken = default)
    {
        var result = await WithRetryStringAsync(apiKey, async client =>
            await CallClaude(client,
                $"For the English word \"{word}\", provide synonyms and antonyms. Reply ONLY in this format:\nSYN: word1, word2, word3\nANT: word1, word2, word3\nMax 5 each. If none, write NONE.",
                100, cancellationToken),
            $"Synonyms({word})", cancellationToken);

        if (string.IsNullOrWhiteSpace(result)) return (null, null);

        string? synonyms = null, antonyms = null;
        foreach (var line in result.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            var trimmed = line.Trim();
            if (trimmed.StartsWith("SYN:", StringComparison.OrdinalIgnoreCase))
            {
                var val = trimmed[4..].Trim();
                if (!val.Equals("NONE", StringComparison.OrdinalIgnoreCase)) synonyms = val;
            }
            else if (trimmed.StartsWith("ANT:", StringComparison.OrdinalIgnoreCase))
            {
                var val = trimmed[4..].Trim();
                if (!val.Equals("NONE", StringComparison.OrdinalIgnoreCase)) antonyms = val;
            }
        }
        return (synonyms, antonyms);
    }
}
