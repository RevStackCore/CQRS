using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RevStackCore.CQRS.Domain;
using RevStackCore.CQRS.Event;
using RevStackCore.CQRS.Snapshot;
using RevStackCore.CQRS.Storage;
using RevStackCore.CQRS.Util;


namespace RevStackCore.CQRS.Repository
{
    public class Repository : IRepository
    {
        private readonly IEventStore _eventStore;
        private readonly ISnapshotStore _snapshotStore;

        public Repository(IEventStore eventStore, ISnapshotStore snapshotStore)
        {
            _eventStore = eventStore;
            _snapshotStore = snapshotStore;
        }

        public virtual async Task<T> GetByIdAsync<T>(int id) where T : AggregateRoot
        {
            T item = default(T);

            var isSnapshottable = typeof(ISnapshottable).GetTypeInfo().IsAssignableFrom(typeof(T));
            Snapshot.Snapshot snapshot = null;

            if ((isSnapshottable) && (_snapshotStore != null))
            {
                snapshot = await _snapshotStore.GetSnapshotAsync(typeof(T), id);
            }

            if (snapshot != null)
            {
                item = ReflectionHelper.CreateInstance<T>();
                ((ISnapshottable)item).ApplySnapshot(snapshot);
                //var events = await _eventStore.GetEventsAsync(typeof(T), id, snapshot.Version + 1, int.MaxValue);
                var events = await _eventStore.GetAsync<T>(id, snapshot.Version + 1);
                item.LoadsFromHistory(events);
            }
            else
            {
                //var events = (await _eventStore.GetEventsAsync(typeof(T), id, 0, int.MaxValue)).ToList();
                var events = await _eventStore.GetAsync<T>(id, 0);

                if (events.Any())
                {
                    item = ReflectionHelper.CreateInstance<T>();
                    item.LoadsFromHistory(events);
                }
            }

            return item;
        }

        public virtual async Task SaveAsync<T>(T aggregate) where T : AggregateRoot
        {
            if (aggregate.HasUncommittedChanges())
            {
                await CommitChanges(aggregate);
            }
        }

        private async Task CommitChanges<T>(T aggregate) where T : AggregateRoot
        {
            var changesToCommit = aggregate.GetUncommittedChanges().ToList();

            foreach (var e in changesToCommit)
            {
                DoPreCommitTasks(e);
            }

            //CommitAsync events to storage provider
            await _eventStore.SaveAsync<T>(aggregate.GetUncommittedChanges());

            //If the Aggregate implements snaphottable
            var snapshottable = aggregate as ISnapshottable;

            if ((snapshottable != null) && (_snapshotStore != null))
            {
                await _snapshotStore.SaveSnapshotAsync(aggregate.GetType(), snapshottable.TakeSnapshot());
            }

            aggregate.MarkChangesAsCommitted();
        }

        private static void DoPreCommitTasks(IEvent e)
        {
            e.EventCommittedTimestamp = DateTime.UtcNow;
        }
    }
}
