using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class SenseAntonym : Entity
{
    public Guid SenseId1 { get; private set; }
    public Guid SenseId2 { get; private set; }
    public SynonymStrength Strength { get; private set; }

    private SenseAntonym() { }

    public static SenseAntonym Create(Guid senseId1, Guid senseId2, SynonymStrength strength)
    {
        var (first, second) = senseId1.CompareTo(senseId2) < 0
            ? (senseId1, senseId2)
            : (senseId2, senseId1);

        return new SenseAntonym
        {
            SenseId1 = first,
            SenseId2 = second,
            Strength = strength
        };
    }
}
