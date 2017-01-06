

namespace RevStackCore.CQRS.Snapshot
{
    public interface ISnapshottable
    {
        Snapshot TakeSnapshot();
        void ApplySnapshot(Snapshot snapshot);
    }
}
