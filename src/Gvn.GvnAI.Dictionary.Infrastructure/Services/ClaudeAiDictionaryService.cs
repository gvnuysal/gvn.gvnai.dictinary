using System.Text.Json;
using Anthropic.SDK;
using Anthropic.SDK.Messaging;
using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Application.DTOs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Services;

public class ClaudeAiDictionaryService(
    IConfiguration configuration,
    ILogger<ClaudeAiDictionaryService> logger) : IAiDictionaryService
{
    public async Task<WordEnrichmentResult?> EnrichWordAsync(
        string lemma, string languageCode, string targetLanguageCode,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var apiKey = configuration["Claude:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                logger.LogWarning("Claude API key is not configured");
                return null;
            }

            var client = new AnthropicClient(apiKey);
            var model = configuration["Claude:Model"] ?? "claude-sonnet-4-20250514";

            var prompt = $$"""
                You are a bilingual dictionary expert. Analyze the word "{{lemma}}" in {{languageCode}}.
                Translate meanings to {{targetLanguageCode}}.

                Return ONLY valid JSON in this exact format (no markdown, no code blocks):
                {
                    "senses": [
                        {
                            "definition": "definition in source language",
                            "definitionShort": "brief definition or null",
                            "registerCode": "formal/informal/slang or null",
                            "domainCode": "subject domain code or null",
                            "translations": [
                                {
                                    "text": "translated word/phrase",
                                    "targetLanguageCode": "{{targetLanguageCode}}",
                                    "partOfSpeechCode": "NOUN/VERB/ADJ etc or null",
                                    "equivalenceType": "Exact/Near/Loose/Gap",
                                    "confidenceScore": 0.95
                                }
                            ],
                            "examples": [
                                {
                                    "sourceText": "example in source language",
                                    "targetText": "example translation or null"
                                }
                            ]
                        }
                    ],
                    "pronunciations": [
                        {
                            "ipa": "/IPA transcription/",
                            "variant": "variant name or null",
                            "isStandard": true
                        }
                    ],
                    "etymology": "etymology text or null"
                }

                Provide 1-5 senses with translations and examples.
                ConfidenceScore must be between 0.0 and 1.0.
                If some fields have no data, use empty arrays or null.
                """;

            var response = await client.Messages.GetClaudeMessageAsync(new MessageParameters
            {
                Model = model,
                MaxTokens = 2048,
                Messages = [new Message(RoleType.User, prompt)]
            }, ctx: cancellationToken);

            var responseText = response.Content
                .OfType<TextContent>()
                .FirstOrDefault()?.Text;

            if (string.IsNullOrWhiteSpace(responseText))
                return null;

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<WordEnrichmentResult>(responseText, options);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to enrich word '{Lemma}' via Claude AI", lemma);
            return null;
        }
    }
}
