using System.Threading.Tasks;
using RevStackCore.CQRS.Command;

namespace RevStackCore.CQRS.Messaging
{
    public interface ICommandBus
    {
        Task<ICommandPublishResponse> ExecuteAsync<T>(T command) where T : ICommand;
    }
}
