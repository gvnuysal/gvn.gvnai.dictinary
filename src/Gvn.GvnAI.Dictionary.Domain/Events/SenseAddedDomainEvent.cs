using Gvn.GvnFramework.Domain.Events;

namespace Gvn.GvnAI.Dictionary.Domain.Events;

public sealed record SenseAddedDomainEvent(Guid WordId, Guid SenseId) : DomainEvent;
