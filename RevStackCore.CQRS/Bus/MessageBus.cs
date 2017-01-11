using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RevStackCore.CQRS.Command;
using RevStackCore.CQRS.Event;
using RevStackCore.CQRS.Exception;
using RevStackCore.CQRS.Message;


namespace RevStackCore.CQRS.Bus
{
    public class MessageBus : IMessageBus
    {
        private readonly Dictionary<Type, List<Action<IMessage>>> _routes = new Dictionary<Type, List<Action<IMessage>>>();

        public void RegisterHandler<T>(Action<T> handler) where T : IMessage
        {
            List<Action<IMessage>> handlers;
            if (!_routes.TryGetValue(typeof(T), out handlers))
            {
                handlers = new List<Action<IMessage>>();
                _routes.Add(typeof(T), handlers);
            }
            handlers.Add((x => handler((T)x)));
        }

        public Task SendAsync<T>(T command) where T : ICommand
        {
            Send(command);
            return Task.FromResult(true);
        }

        public void Send<T>(T command) where T : ICommand
        {
            List<Action<IMessage>> handlers;
            if (_routes.TryGetValue(command.GetType(), out handlers))
            {
                if (handlers.Count != 1)
                {
                    throw new CommandExecutionFailedException($"Cannot send to more than one handler for {typeof(T)}");
                }

                try
                {
                    handlers[0](command);
                }
                catch (System.Exception ex)
                {
                    throw new CommandExecutionFailedException(ex.Message);
                }
                
            }
            else
            {
                throw new CommandExecutionFailedException($"Command handler not found for {typeof(T)}");
            }
        }

        public Task PublishAsync(IEvent @event)
        {
            Publish(@event);
            return Task.FromResult(true);
        }

        public void Publish(IEvent @event)
        {
            List<Action<IMessage>> handlers;
            if (!_routes.TryGetValue(@event.GetType(), out handlers))
            {
                return;
            }

            foreach (var handler in handlers)
            {
                handler(@event);
            }
        }

    }
}
