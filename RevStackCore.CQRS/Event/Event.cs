using System;


namespace RevStackCore.CQRS.Event
{
    public class Event : IEvent
    {
        public Guid Id { get; private set; }
        public Guid AggregateId { get; set; }
        public string Name { get; protected set; }
        public int Version { get; set; }
        public DateTime EventCommittedTimestamp  { get; set; }

        public Event(Guid aggregateId, int version)
        {
            Id = Guid.NewGuid();
            Name = this.GetType().Name;
            AggregateId = aggregateId;
            Version = version;
        }
    }
}
