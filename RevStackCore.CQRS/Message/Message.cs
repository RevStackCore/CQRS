using System;


namespace RevStackCore.CQRS.Message
{
    public class Message : IMessage
    {
        public Guid Id { get; }
    }
}
