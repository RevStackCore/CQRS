

namespace RevStackCore.CQRS.Exception
{
    public class AggregateStateMismatchException : System.Exception
    {
        public AggregateStateMismatchException(string msg) : base(msg)
        {

        }
    }
}
