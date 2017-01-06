using System;
using System.Collections.Generic;
using System.Linq;
using RevStackCore.CQRS.Event;
using RevStackCore.CQRS.Util;

namespace RevStackCore.CQRS.Domain
{
    public class AggregateBase : IAggregate
    {
        private readonly List<IEvent> _uncommittedChanges;

        public AggregateBase()
        {
            _uncommittedChanges = new List<IEvent>();
        }

        public Guid Id { get; set; }
        public int Version { get; protected set; }
        public int EventVersion { get; protected set; }

        public bool HasUncommittedChanges()
        {
            lock (_uncommittedChanges)
            {
                return _uncommittedChanges.Any();
            }
        }

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            lock (_uncommittedChanges)
            {
                return _uncommittedChanges.ToList();
            }
        }

        public void MarkChangesAsCommitted()
        {
            lock (_uncommittedChanges)
            {
                _uncommittedChanges.Clear();
            }
        }

        public void LoadsFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history) ApplyEvent(e, false);
            Version = history.Last().Version;
            EventVersion = Version;
        }

        protected void ApplyEvent(IEvent @event)
        {
            ApplyEvent(@event, true);
        }

        private void ApplyEvent(IEvent @event, bool isNew)
        {
            @event.InvokeOnAggregate(this, "Handle");

            Version++;

            if (isNew)
            {
                lock (_uncommittedChanges)
                {
                    _uncommittedChanges.Add(@event);
                }
            }


        }
    }
}
