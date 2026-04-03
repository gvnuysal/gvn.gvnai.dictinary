using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class Etymology : Entity
{
    public Guid WordId { get; private set; }
    public Guid? OriginLanguageId { get; private set; }
    public string Text { get; private set; } = null!;

    private Etymology() { }

    public static Etymology Create(Guid? originLanguageId, string text, Guid wordId)
    {
        return new Etymology
        {
            WordId = wordId,
            OriginLanguageId = originLanguageId,
            Text = text
        };
    }
}
