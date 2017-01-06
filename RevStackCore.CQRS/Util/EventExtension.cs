using System;
using RevStackCore.CQRS.Domain;
using RevStackCore.CQRS.Event;
using RevStackCore.CQRS.Exception;


namespace RevStackCore.CQRS.Util
{
    public static class EventExtension
    {
        public static void InvokeOnAggregate(this IEvent @event, AggregateBase aggregate, string methodName)
        {
            var method = ReflectionHelper.GetMethod(aggregate.GetType(), methodName, new Type[] { @event.GetType() }); //Find the right method

            if (method != null)
            {
                method.Invoke(aggregate, new object[] { @event }); //invoke with the event as argument
            }
            else
            {
                throw new AggregateEventOnApplyMethodMissingException($"No event Apply method found on {aggregate.GetType()} for {@event.GetType()}");
            }
        }
    }
}
