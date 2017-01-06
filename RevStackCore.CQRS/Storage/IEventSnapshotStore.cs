using System;
using System.Threading.Tasks;


namespace RevStackCore.CQRS.Storage
{
    public interface IEventSnapshotStore : ISnapshotStore
    {
        Task<Snapshot.Snapshot> GetSnapshotAsync(Type aggregateType, Guid aggregateId, int version);
    }
}
