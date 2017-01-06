using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RevStackCore.CQRS.Domain;
using RevStackCore.CQRS.Event;

namespace RevStackCore.CQRS.Storage
{
    public interface IEventStore
    {
        Task<IEnumerable<IEvent>> GetEventsAsync(Type aggregateType, Guid aggregateId, int start, int count);
        Task<IEvent> GetLastEventAsync(Type aggregateType, Guid aggregateId);
        Task CommitChangesAsync(AggregateBase aggregate);
    }
}
