using Gvn.GvnAI.Dictionary.Domain.Shared.Enums;
using Gvn.GvnFramework.Domain.Entities;

namespace Gvn.GvnAI.Dictionary.Domain.Entities;

public class SenseSynonym : Entity
{
    public Guid SenseId1 { get; private set; }
    public Guid SenseId2 { get; private set; }
    public SynonymStrength Strength { get; private set; }

    private SenseSynonym() { }

    public static SenseSynonym Create(Guid senseId1, Guid senseId2, SynonymStrength strength)
    {
        // Canonical ordering to prevent duplicate pairs
        var (first, second) = senseId1.CompareTo(senseId2) < 0
            ? (senseId1, senseId2)
            : (senseId2, senseId1);

        return new SenseSynonym
        {
            SenseId1 = first,
            SenseId2 = second,
            Strength = strength
        };
    }
}
