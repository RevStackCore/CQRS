
using System.Threading.Tasks;


namespace RevStackCore.CQRS.Message
{
    public interface IHandler<in T> where T : IMessage
    {
        Task ExecuteAsync(T message);
    }
}
