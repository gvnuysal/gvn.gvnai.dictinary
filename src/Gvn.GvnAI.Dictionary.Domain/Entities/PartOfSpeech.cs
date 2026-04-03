using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class PartOfSpeech : Entity
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string Abbreviation { get; private set; } = null!;

    private PartOfSpeech() { }

    public static PartOfSpeech Create(string code, string name, string abbreviation)
    {
        return new PartOfSpeech
        {
            Code = code.ToUpperInvariant(),
            Name = name,
            Abbreviation = abbreviation
        };
    }

    public void Update(string name, string abbreviation)
    {
        Name = name;
        Abbreviation = abbreviation;
    }
}
