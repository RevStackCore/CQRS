using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using RevStackCore.CQRS.Domain;
using RevStackCore.CQRS.Event;
using RevStackCore.CQRS.Messaging;
using RevStackCore.CQRS.Snapshot;
using RevStackCore.CQRS.Storage;
using RevStackCore.CQRS.Util;


namespace RevStackCore.CQRS.Repository
{
    public class Repository : IRepository
    {
        public IEventStore EventStore { get; }
        public ISnapshotStore SnapshotStore { get; }
        public IEventPublisher EventPublisher { get; }

        public Repository(IEventStore eventStore, ISnapshotStore snapshotStore, IEventPublisher eventPublisher)
        {
            EventStore = eventStore;
            SnapshotStore = snapshotStore;
            EventPublisher = eventPublisher;
        }

        public T GetById<T>(Guid id) where T : AggregateBase
        {
            return Task.Run(() => GetByIdAsync<T>(id)).Result;
        }

        public virtual async Task<T> GetByIdAsync<T>(Guid id) where T : AggregateBase
        {
            T item = default(T);

            var isSnapshottable = typeof(ISnapshottable).GetTypeInfo().IsAssignableFrom(typeof(T));
            Snapshot.Snapshot snapshot = null;

            if ((isSnapshottable) && (SnapshotStore != null))
            {
                snapshot = await SnapshotStore.GetSnapshotAsync(typeof(T), id);
            }

            if (snapshot != null)
            {
                item = ReflectionHelper.CreateInstance<T>();
                ((ISnapshottable)item).ApplySnapshot(snapshot);
                var events = await EventStore.GetEventsAsync(typeof(T), id, snapshot.Version + 1, int.MaxValue);
                item.LoadsFromHistory(events);
            }
            else
            {
                var events = (await EventStore.GetEventsAsync(typeof(T), id, 0, int.MaxValue)).ToList();

                if (events.Any())
                {
                    item = ReflectionHelper.CreateInstance<T>();
                    item.LoadsFromHistory(events);
                }
            }

            return item;
        }

        public void Save<T>(T aggregate) where T : AggregateBase
        {
            Task.Run(() => SaveAsync(aggregate));
        }

        public virtual async Task SaveAsync<T>(T aggregate) where T : AggregateBase
        {
            if (aggregate.HasUncommittedChanges())
            {
                await CommitChanges(aggregate);
            }
        }

        private async Task CommitChanges(AggregateBase aggregate)
        {
            //IEvent item = await EventStore.GetLastEventAsync(aggregate.GetType(), aggregate.Id);

            var changesToCommit = aggregate.GetUncommittedChanges().ToList();

            foreach (var e in changesToCommit)
            {
                DoPreCommitTasks(e);
            }

            //CommitAsync events to storage provider
            await EventStore.CommitChangesAsync(aggregate);

            //Publish to event publisher asynchronously
            foreach (var e in changesToCommit)
            {
                await EventPublisher.PublishAsync(e);
            }

            //If the Aggregate implements snaphottable
            var snapshottable = aggregate as ISnapshottable;

            if ((snapshottable != null) && (SnapshotStore != null))
            {
                await SnapshotStore.SaveSnapshotAsync(aggregate.GetType(), snapshottable.TakeSnapshot());
                //Every N events we save a snapshot
                //if ((aggregate.Version >= SnapshotStore.SnapshotFrequency) &&
                //        (
                //            (changesToCommit.Count >= SnapshotStore.SnapshotFrequency) ||
                //            (aggregate.Version % SnapshotStore.SnapshotFrequency < changesToCommit.Count) ||
                //            (aggregate.Version % SnapshotStore.SnapshotFrequency == 0)
                //        )
                //    )
                //{
                //    await SnapshotStore.SaveSnapshotAsync(aggregate.GetType(), snapshottable.TakeSnapshot());
                //}
            }

            aggregate.MarkChangesAsCommitted();
        }

        private static void DoPreCommitTasks(IEvent e)
        {
            e.EventCommittedTimestamp = DateTime.UtcNow;
        }
    }
}
