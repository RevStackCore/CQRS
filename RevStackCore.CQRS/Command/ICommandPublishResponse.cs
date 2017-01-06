using System;


namespace RevStackCore.CQRS.Command
{
    public interface ICommandPublishResponse
    {
        bool Success { get; }
        Guid AggregateId { get; }
        Guid RequestId { get; set; }
        string Message { get; set; }
        System.Exception ResultException { get; }
        void EnsurePublished();
    }
}
