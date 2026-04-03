using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class Language : Entity
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public string NativeName { get; private set; } = null!;
    public TextDirection Direction { get; private set; }

    private Language() { }

    public static Language Create(string code, string name, string nativeName, TextDirection direction = TextDirection.LTR)
    {
        return new Language
        {
            Code = code.ToLowerInvariant(),
            Name = name,
            NativeName = nativeName,
            Direction = direction
        };
    }

    public void Update(string name, string nativeName, TextDirection direction)
    {
        Name = name;
        NativeName = nativeName;
        Direction = direction;
    }
}
