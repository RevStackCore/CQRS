using System;


namespace RevStackCore.CQRS.Event
{
    public class Event : IEvent
    {
        public Guid Id { get; set; }
        public int AggregateId { get; set; }
        public string Name { get; protected set; }
        public int Version { get; set; }
        public DateTime EventCommittedTimestamp  { get; set; }

        public Event(int aggregateId, int version)
        {
            Id = Guid.NewGuid();
            Name = this.GetType().Name;
            AggregateId = aggregateId;
            Version = version;
        }
    }
}
