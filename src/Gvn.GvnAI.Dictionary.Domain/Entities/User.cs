using Gvn.GvnFramework.Domain.Aggregates;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class User : AggregateRoot
{
    public string Email { get; private set; } = null!;
    public string PasswordHash { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public string Role { get; private set; } = "User";

    // AI/Translation ayarları (kullanıcı bazlı)
    public string TranslateProvider { get; private set; } = "claude"; // "claude" veya "google"
    public string? ClaudeApiKey { get; private set; }
    public string? GoogleTranslateApiKey { get; private set; }
    public bool QuizAutoSpeak { get; private set; } = true; // Oyunda kelime gelince otomatik seslendir

    private User() { }

    public static User Create(string email, string passwordHash, string fullName)
    {
        return new User
        {
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash,
            FullName = fullName
        };
    }

    public void UpdateProfile(string fullName)
    {
        FullName = fullName;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
    }

    public void UpdateApiSettings(string translateProvider, string? claudeApiKey, string? googleTranslateApiKey, bool quizAutoSpeak)
    {
        TranslateProvider = translateProvider;
        ClaudeApiKey = claudeApiKey;
        GoogleTranslateApiKey = googleTranslateApiKey;
        QuizAutoSpeak = quizAutoSpeak;
    }
}
