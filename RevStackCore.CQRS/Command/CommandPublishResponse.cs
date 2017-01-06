using System;
using RevStackCore.CQRS.Exception;


namespace RevStackCore.CQRS.Command
{
    public class CommandPublishResponse : ICommandPublishResponse
    {
        public static CommandPublishResponse Ok = new CommandPublishResponse { Success = true };
        public static CommandPublishResponse Fail = new CommandPublishResponse { Success = false };

        public CommandPublishResponse(bool success  = false, Guid aggregateId = default(Guid))
        {
            Success = success;
            AggregateId = aggregateId;
            Message = String.Empty;
        }

        public CommandPublishResponse(bool success, Guid aggregateId, System.Exception resultException)
        {
            Success = success;
            AggregateId = aggregateId;
            Message = String.Empty;
            ResultException = resultException;
        }

        public bool Success { get; private set; }
        public Guid AggregateId { get; private set; }
        public Guid RequestId { get; set; }
        public string Message { get; set; }
        public System.Exception ResultException { get; }

        public void EnsurePublished()
        {
            if (this.Success == false)
            {
                throw new CommandExecutionFailedException(
                    $"Command failed with message: {this.Message} \n\n {this.ResultException?.Message}");
            }
        }
    }
}
