using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class Register : Entity
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    private Register() { }

    public static Register Create(string code, string name)
    {
        return new Register
        {
            Code = code.ToLowerInvariant(),
            Name = name
        };
    }

    public void Update(string name)
    {
        Name = name;
    }
}
