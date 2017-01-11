using System;
using System.Collections.Generic;
using RevStackCore.CQRS.Event;


namespace RevStackCore.CQRS.Domain
{
    public interface IAggregate 
    {
        int Id { get; }
        int Version { get; }
        bool HasUncommittedChanges();
        IEnumerable<IEvent> GetUncommittedChanges();
        void MarkChangesAsCommitted();
    }
}
