using RevStackCore.CQRS.Message;


namespace RevStackCore.CQRS.Command
{
    public interface ICommandHandler<in T> : IHandler<T> where T : ICommand
    {
        
    }
}
