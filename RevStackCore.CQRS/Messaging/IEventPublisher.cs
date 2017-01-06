using System.Threading.Tasks;
using RevStackCore.CQRS.Event;

namespace RevStackCore.CQRS.Messaging
{
    public interface IEventPublisher
    {
        Task PublishAsync(IEvent @event);
    }
}
