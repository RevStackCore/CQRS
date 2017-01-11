using System;


namespace RevStackCore.CQRS.Snapshot
{
    public class Snapshot
    {
        public Guid Id { get; set; }
        public Type AggregateType { get; set; }
        public int AggregateId { get; set; }
        public int Version { get; set; }

        public Snapshot()
        {
        }

        public Snapshot(Guid id, int aggregateId, int version) 
        {
            Id = id;
            AggregateId = aggregateId;
            Version = version;
        }
    }
}
