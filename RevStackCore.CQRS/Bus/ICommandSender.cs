using System.Threading.Tasks;
using RevStackCore.CQRS.Command;


namespace RevStackCore.CQRS.Bus
{
    public interface ICommandSender
    {
        void Send<T>(T command) where T : ICommand;
        Task SendAsync<T>(T command) where T : ICommand;
    }
}
