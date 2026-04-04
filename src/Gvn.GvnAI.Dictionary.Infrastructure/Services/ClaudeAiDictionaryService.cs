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

    public async Task<WordEnrichmentResult?> EnrichWordAsync(
        string apiKey, string lemma, string languageCode, string targetLanguageCode,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey)) { logger.LogWarning("Claude API key not provided"); return null; }
        try
        {
            var client = new AnthropicClient(apiKey);
            var prompt = $$"""
                You are a bilingual dictionary expert. Analyze the word "{{lemma}}" in {{languageCode}}.
                Translate meanings to {{targetLanguageCode}}.
                Return ONLY valid JSON in this exact format (no markdown, no code blocks):
                {
                    "senses": [{"definition":"...","definitionShort":"...","registerCode":null,"domainCode":null,
                      "translations":[{"text":"...","targetLanguageCode":"{{targetLanguageCode}}","partOfSpeechCode":null,"equivalenceType":"Exact","confidenceScore":0.95}],
                      "examples":[{"sourceText":"...","targetText":"..."}]}],
                    "pronunciations": [{"ipa":"/.../ ","variant":null,"isStandard":true}],
                    "etymology": "..."
                }
                Provide 1-5 senses. ConfidenceScore 0.0-1.0. Empty arrays or null for missing data.
                """;

            var response = await client.Messages.GetClaudeMessageAsync(new MessageParameters
            {
                Model = DefaultModel, MaxTokens = 2048,
                Messages = [new Message(RoleType.User, prompt)]
            }, ctx: cancellationToken);

            var text = response.Content.OfType<TextContent>().FirstOrDefault()?.Text;
            if (string.IsNullOrWhiteSpace(text)) return null;

            return JsonSerializer.Deserialize<WordEnrichmentResult>(text, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex) { logger.LogError(ex, "Enrich failed for '{Lemma}'", lemma); return null; }
    }

    public async Task<string?> GetDefinitionAsync(string apiKey, string word, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey)) return null;
        try
        {
            var client = new AnthropicClient(apiKey);
            var response = await client.Messages.GetClaudeMessageAsync(new MessageParameters
            {
                Model = DefaultModel, MaxTokens = 150,
                Messages = [new Message(RoleType.User, $"Define the English word \"{word}\" in one clear, concise sentence. Return ONLY the definition text.")]
            }, ctx: cancellationToken);
            return response.Content.OfType<TextContent>().FirstOrDefault()?.Text?.Trim();
        }
        catch (Exception ex) { logger.LogError(ex, "Definition failed for '{Word}'", word); return null; }
    }

    public async Task<string?> TranslateWordAsync(string apiKey, string word, string targetLang, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey)) return null;
        try
        {
            var client = new AnthropicClient(apiKey);
            var langName = targetLang == "tr" ? "Turkish" : targetLang;
            var response = await client.Messages.GetClaudeMessageAsync(new MessageParameters
            {
                Model = DefaultModel, MaxTokens = 50,
                Messages = [new Message(RoleType.User, $"Translate the English word \"{word}\" to {langName}. Return ONLY the translated word(s). Max 3, comma separated.")]
            }, ctx: cancellationToken);
            return response.Content.OfType<TextContent>().FirstOrDefault()?.Text?.Trim();
        }
        catch (Exception ex) { logger.LogError(ex, "Translate failed for '{Word}'", word); return null; }
    }

    public async Task<string?> DetectPartOfSpeechAsync(string apiKey, string word, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey)) return null;
        try
        {
            var client = new AnthropicClient(apiKey);
            var response = await client.Messages.GetClaudeMessageAsync(new MessageParameters
            {
                Model = DefaultModel, MaxTokens = 10,
                Messages = [new Message(RoleType.User, $"What is the primary part of speech of \"{word}\"? Reply ONLY: NOUN, VERB, ADJ, ADV, PRON, PREP, CONJ, INTERJ")]
            }, ctx: cancellationToken);
            return response.Content.OfType<TextContent>().FirstOrDefault()?.Text?.Trim().ToUpperInvariant();
        }
        catch (Exception ex) { logger.LogError(ex, "POS detection failed for '{Word}'", word); return null; }
    }
}
