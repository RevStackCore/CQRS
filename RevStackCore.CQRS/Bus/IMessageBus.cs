

using System;
using RevStackCore.CQRS.Message;

namespace RevStackCore.CQRS.Bus
{
    public interface IMessageBus : ICommandSender, IEventPublisher
    {
        void RegisterHandler<T>(Action<T> handler) where T : IMessage;
    }

}
