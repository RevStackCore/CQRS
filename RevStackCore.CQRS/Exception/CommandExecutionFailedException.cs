

namespace RevStackCore.CQRS.Exception
{
    public class CommandExecutionFailedException : System.Exception
    {
        public CommandExecutionFailedException(string msg) : base(msg)
        {

        }
    }
}
