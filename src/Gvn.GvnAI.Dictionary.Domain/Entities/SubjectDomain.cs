using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class SubjectDomain : Entity
{
    public string Code { get; private set; } = null!;
    public string Name { get; private set; } = null!;

    private SubjectDomain() { }

    public static SubjectDomain Create(string code, string name)
    {
        return new SubjectDomain
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
