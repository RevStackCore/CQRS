

namespace RevStackCore.CQRS.Exception
{
    public class AggregateNotFoundException : System.Exception
    {
        public AggregateNotFoundException(string msg) : base(msg)
        {

        }
    }
}
