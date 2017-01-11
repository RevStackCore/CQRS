using System;
using RevStackCore.CQRS.Exception;


namespace RevStackCore.CQRS.Command
{
    public class CommandResponse
    {
        public static CommandResponse Ok = new CommandResponse { Success = true };
        public static CommandResponse Fail = new CommandResponse { Success = false };

        public CommandResponse(bool success  = false, int aggregateId = -1)
        {
            Success = success;
            AggregateId = aggregateId;
            Message = String.Empty;
        }

        public CommandResponse(bool success, int aggregateId, System.Exception resultException)
        {
            Success = success;
            AggregateId = aggregateId;
            Message = String.Empty;
            ResultException = resultException;
        }

        public bool Success { get; private set; }
        public int AggregateId { get; private set; }
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
