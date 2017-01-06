using System;


namespace RevStackCore.CQRS.Attribute
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class InternalEventHandler : System.Attribute
    {
    }
}
