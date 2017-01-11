using System.Collections.Generic;
using System.Linq;
using RevStackCore.CQRS.Event;
using RevStackCore.CQRS.Util;


namespace RevStackCore.CQRS.Domain
{
    public class AggregateRoot : IAggregate
    {
        private readonly List<IEvent> _uncommittedChanges;

        public AggregateRoot()
        {
            _uncommittedChanges = new List<IEvent>();
        }

        public int Id { get; set; }
        public int Version { get; protected set; }

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
                Version = Version + _uncommittedChanges.Count;
                _uncommittedChanges.Clear();
            }
        }

        public void LoadsFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history) ApplyChange(e, false);
            Version = history.Last().Version;
        }

        protected void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, true);
        }

        private void ApplyChange(IEvent @event, bool isNew)
        {
            lock (_uncommittedChanges)
            {
                @event.InvokeOnAggregate(this, "Handle");

                if (isNew)
                {
                    _uncommittedChanges.Add(@event);
                }

                Version++;
            }
        }
    }
}
