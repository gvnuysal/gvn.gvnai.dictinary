using System.Security.Claims;
using Gvn.GvnAI.Dictionary.Application.Abstractions;
using Gvn.GvnAI.Dictionary.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gvn.GvnAI.Dictionary.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class TranslateController(
    ITranslateService googleTranslateService,
    IAiDictionaryService aiService,
    IUserRepository userRepository) : ControllerBase
{
    [HttpGet("define")]
    public async Task<IActionResult> Define([FromQuery] string word)
    {
        if (string.IsNullOrWhiteSpace(word)) return BadRequest("Word is required.");
        var user = await userRepository.GetByIdAsync(GetUserId());
        var key = user?.ClaudeApiKey;
        if (string.IsNullOrWhiteSpace(key)) return BadRequest(new { message = "Claude API key ayarlanmamis. Profil sayfasindan ekleyin." });

        var definition = await aiService.GetDefinitionAsync(key, word);
        return definition is null ? StatusCode(503, new { message = "Definition service unavailable." }) : Ok(new { definition });
    }

    [HttpGet("ai-translate")]
    public async Task<IActionResult> AiTranslate([FromQuery] string word, [FromQuery] string target = "tr")
    {
        if (string.IsNullOrWhiteSpace(word)) return BadRequest("Word is required.");
        var user = await userRepository.GetByIdAsync(GetUserId());
        var key = user?.ClaudeApiKey;
        if (string.IsNullOrWhiteSpace(key)) return BadRequest(new { message = "Claude API key ayarlanmamis. Profil sayfasindan ekleyin." });

        var translation = await aiService.TranslateWordAsync(key, word, target);
        return translation is null ? StatusCode(503, new { message = "AI translation unavailable." }) : Ok(new { translatedText = translation });
    }

    [HttpGet("detect-pos")]
    public async Task<IActionResult> DetectPartOfSpeech([FromQuery] string word)
    {
        if (string.IsNullOrWhiteSpace(word)) return BadRequest("Word is required.");
        var user = await userRepository.GetByIdAsync(GetUserId());
        var key = user?.ClaudeApiKey;
        if (string.IsNullOrWhiteSpace(key)) return BadRequest(new { message = "Claude API key ayarlanmamis." });

        var posCode = await aiService.DetectPartOfSpeechAsync(key, word);
        return posCode is null ? StatusCode(503, new { message = "POS detection unavailable." }) : Ok(new { posCode });
    }

    [HttpGet("google")]
    public async Task<IActionResult> GoogleTranslate([FromQuery] string text, [FromQuery] string source = "en", [FromQuery] string target = "tr")
    {
        if (string.IsNullOrWhiteSpace(text)) return BadRequest("Text is required.");
        var user = await userRepository.GetByIdAsync(GetUserId());
        var key = user?.GoogleTranslateApiKey;
        if (string.IsNullOrWhiteSpace(key)) return BadRequest(new { message = "Google Translate API key ayarlanmamis. Profil sayfasindan ekleyin." });

        var result = await googleTranslateService.TranslateAsync(key, text, source, target);
        return result is null ? StatusCode(503, new { message = "Google Translate unavailable." }) : Ok(new { translatedText = result });
    }

    [HttpGet("auto")]
    public async Task<IActionResult> AutoTranslate([FromQuery] string word, [FromQuery] string target = "tr")
    {
        if (string.IsNullOrWhiteSpace(word)) return BadRequest("Word is required.");
        var user = await userRepository.GetByIdAsync(GetUserId());
        if (user is null) return Unauthorized();

        string? result;
        var provider = user.TranslateProvider;

        if (provider == "google" && !string.IsNullOrWhiteSpace(user.GoogleTranslateApiKey))
            result = await googleTranslateService.TranslateAsync(user.GoogleTranslateApiKey, word, "en", target);
        else if (!string.IsNullOrWhiteSpace(user.ClaudeApiKey))
            result = await aiService.TranslateWordAsync(user.ClaudeApiKey, word, target);
        else
            return BadRequest(new { message = "API key ayarlanmamis. Profil sayfasindan ekleyin." });

        return result is null ? StatusCode(503, new { message = "Translation unavailable." }) : Ok(new { translatedText = result, provider });
    }

    [HttpGet("synonyms")]
    public async Task<IActionResult> GetSynonyms([FromQuery] string word)
    {
        if (string.IsNullOrWhiteSpace(word)) return BadRequest("Word is required.");
        var user = await userRepository.GetByIdAsync(GetUserId());
        var key = user?.ClaudeApiKey;
        if (string.IsNullOrWhiteSpace(key)) return BadRequest(new { message = "Claude API key ayarlanmamis." });

        var (synonyms, antonyms) = await aiService.GetSynonymsAsync(key, word);
        return Ok(new { synonyms, antonyms });
    }

    private Guid GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return Guid.Parse(claim!.Value);
    }
}
