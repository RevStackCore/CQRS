using System;
using System.Threading.Tasks;
using RevStackCore.CQRS.Event;


namespace RevStackCore.CQRS.Bus
{
    public interface IEventPublisher
    {
        void Publish(IEvent @event);
        Task PublishAsync(IEvent @event);
    }
}
