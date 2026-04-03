using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class Translation : Entity
{
    public Guid SenseId { get; private set; }
    public Guid TargetLanguageId { get; private set; }
    public string TranslationText { get; private set; } = null!;
    public Guid? PartOfSpeechId { get; private set; }
    public Guid? RegisterId { get; private set; }
    public EquivalenceType EquivalenceType { get; private set; }
    public double ConfidenceScore { get; private set; }

    private Translation() { }

    public static Translation Create(
        Guid senseId, Guid targetLanguageId, string translationText,
        Guid? partOfSpeechId, Guid? registerId,
        EquivalenceType equivalenceType, double confidenceScore)
    {
        return new Translation
        {
            SenseId = senseId,
            TargetLanguageId = targetLanguageId,
            TranslationText = translationText,
            PartOfSpeechId = partOfSpeechId,
            RegisterId = registerId,
            EquivalenceType = equivalenceType,
            ConfidenceScore = Math.Clamp(confidenceScore, 0.0, 1.0)
        };
    }

    public void Update(
        string translationText, Guid? partOfSpeechId, Guid? registerId,
        EquivalenceType equivalenceType, double confidenceScore)
    {
        TranslationText = translationText;
        PartOfSpeechId = partOfSpeechId;
        RegisterId = registerId;
        EquivalenceType = equivalenceType;
        ConfidenceScore = Math.Clamp(confidenceScore, 0.0, 1.0);
    }
}
