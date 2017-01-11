using System;
using RevStackCore.CQRS.Message;


namespace RevStackCore.CQRS.Event
{
    public interface IEvent : IMessage
    {
        int AggregateId { get; set; }
        int Version { get; set; }
        DateTime EventCommittedTimestamp { get; set; }
    }
}
