using Gvn.GvnFramework.Domain.Events;

namespace Gvn.GvnAI.Dictionary.Domain.Events;

public sealed record WordEnrichedDomainEvent(Guid WordId, string Lemma) : DomainEvent;
