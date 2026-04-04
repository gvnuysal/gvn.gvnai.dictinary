using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class Favorite : Entity
{
    public Guid UserId { get; private set; }
    public Guid WordId { get; private set; }
    public DateTime AddedAt { get; private set; }

    private Favorite() { }

    public static Favorite Create(Guid userId, Guid wordId)
    {
        return new Favorite
        {
            UserId = userId,
            WordId = wordId,
            AddedAt = DateTime.UtcNow
        };
    }
}
