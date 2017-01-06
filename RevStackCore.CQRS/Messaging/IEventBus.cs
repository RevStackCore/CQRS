using RevStackCore.CQRS.Event;

namespace RevStackCore.CQRS.Messaging
{
    public interface IEventBus
    {
        void RaiseEvent<T>(T @event) where T : IEvent;
    }
}
