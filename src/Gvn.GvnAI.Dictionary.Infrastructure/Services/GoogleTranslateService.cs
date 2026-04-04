using System.Net.Http.Json;
using System.Text.Json;
using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace Gvn.GvnAI.Dictionary.Infrastructure.Services;

public class GoogleTranslateService(
    IHttpClientFactory httpClientFactory,
    ILogger<GoogleTranslateService> logger) : ITranslateService
{
    public async Task<string?> TranslateAsync(string apiKey, string text, string sourceLang, string targetLang, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(apiKey)) { logger.LogWarning("Google Translate API key not provided"); return null; }
        try
        {
            var client = httpClientFactory.CreateClient();
            var url = $"https://translation.googleapis.com/language/translate/v2?key={apiKey}";
            var request = new { q = text, source = sourceLang, target = targetLang, format = "text" };

            var response = await client.PostAsJsonAsync(url, request, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<JsonElement>(ct);
            return json.GetProperty("data").GetProperty("translations")[0].GetProperty("translatedText").GetString();
        }
        catch (Exception ex) { logger.LogError(ex, "Google Translate failed for '{Text}'", text); return null; }
    }
}
