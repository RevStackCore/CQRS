

namespace RevStackCore.CQRS.Exception
{
    public class ConcurrencyException : System.Exception
    {
        public ConcurrencyException(string msg) : base(msg)
        {

        }
    }
}
