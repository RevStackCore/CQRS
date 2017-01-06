using System;
using System.Collections.Generic;
using RevStackCore.CQRS.Event;


namespace RevStackCore.CQRS.Domain
{
    public interface IAggregate 
    {
        Guid Id { get; }
        int Version { get; }
        int EventVersion { get; }
        bool HasUncommittedChanges();
        IEnumerable<IEvent> GetUncommittedChanges();
        void MarkChangesAsCommitted();
    }
}
