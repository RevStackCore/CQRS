using System.Threading.Tasks;


namespace RevStackCore.CQRS.Event
{
    public interface IEventHandler<T> where T : IEvent
    {
        Task HandleEventAsync(T @event);
    }
}
