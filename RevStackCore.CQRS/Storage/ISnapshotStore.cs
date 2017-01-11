using System;
using System.Threading.Tasks;


namespace RevStackCore.CQRS.Storage
{
    public interface ISnapshotStore
    {
        int SnapshotFrequency { get; }
        Task<Snapshot.Snapshot> GetSnapshotAsync(Type aggregateType, int aggregateId);
        Task SaveSnapshotAsync(Type aggregateType, Snapshot.Snapshot snapshot);
    }
}
