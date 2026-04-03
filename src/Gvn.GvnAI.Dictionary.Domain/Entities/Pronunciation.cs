using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class Pronunciation : Entity
{
    public Guid WordId { get; private set; }
    public string IpaTranscription { get; private set; } = null!;
    public string? Variant { get; private set; }
    public bool IsStandard { get; private set; }

    private Pronunciation() { }

    public static Pronunciation Create(string ipa, string? variant, bool isStandard, Guid wordId)
    {
        return new Pronunciation
        {
            WordId = wordId,
            IpaTranscription = ipa,
            Variant = variant,
            IsStandard = isStandard
        };
    }
}
