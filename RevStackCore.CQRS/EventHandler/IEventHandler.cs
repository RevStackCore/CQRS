using System.Threading.Tasks;
using RevStackCore.CQRS.Event;

namespace RevStackCore.CQRS.EventHandler
{
    public interface IEventHandler<T> where T : IEvent
    {
        Task HandleEventAsync(T @event);
    }
}
