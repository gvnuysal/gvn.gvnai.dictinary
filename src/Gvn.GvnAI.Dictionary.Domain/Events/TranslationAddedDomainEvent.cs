using Gvn.GvnFramework.Domain.Events;

namespace Gvn.GvnAI.Dictionary.Domain.Events;

public sealed record TranslationAddedDomainEvent(Guid SenseId, Guid TranslationId) : DomainEvent;
