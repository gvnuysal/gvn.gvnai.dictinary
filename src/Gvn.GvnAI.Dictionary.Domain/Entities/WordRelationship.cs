using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class WordRelationship : Entity
{
    public Guid SourceWordId { get; private set; }
    public Guid TargetWordId { get; private set; }
    public RelationshipType Type { get; private set; }

    private WordRelationship() { }

    public static WordRelationship Create(Guid sourceWordId, Guid targetWordId, RelationshipType type)
    {
        return new WordRelationship
        {
            SourceWordId = sourceWordId,
            TargetWordId = targetWordId,
            Type = type
        };
    }
}
