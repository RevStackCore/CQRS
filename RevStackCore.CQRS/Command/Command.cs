using System;


namespace RevStackCore.CQRS.Command
{
    public class Command : ICommand
    {
        public Guid Id { get; private set; }
        public string Name { get; protected set; }
        public int AggregateId { get; }
        public int Version { get; private set; }

        public Command(Guid id, int aggregateId, int version)
        {
            Id = id;
            AggregateId = aggregateId;
            Version = version;
        }
    }
}
