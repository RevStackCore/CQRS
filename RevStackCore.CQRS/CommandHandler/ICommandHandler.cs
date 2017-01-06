using System;
using System.Threading.Tasks;
using RevStackCore.CQRS.Command;

namespace RevStackCore.CQRS.CommandHandler
{
    public interface ICommandHandler<T> where T : ICommand
    {
        Task ExecuteAsync(T command);
    }
}
