using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RevStackCore.CQRS.Event;

namespace RevStackCore.CQRS.Storage
{
    public interface IEventStore
    {
        Task SaveAsync<T>(IEnumerable<IEvent> events);
        Task<IEnumerable<IEvent>> GetAsync<T>(int aggregateId, int fromVersion);
    }
}
