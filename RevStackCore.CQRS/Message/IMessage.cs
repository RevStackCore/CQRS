using System;


namespace RevStackCore.CQRS.Message
{
    public interface IMessage
    {
        /// <summary>
        /// Unique message ID
        /// </summary>
        Guid Id { get; }
        string Name { get; }
    }
}
