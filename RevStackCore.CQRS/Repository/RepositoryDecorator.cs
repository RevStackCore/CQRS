using System.Threading.Tasks;
using RevStackCore.CQRS.Domain;


namespace RevStackCore.CQRS.Repository
{
    public abstract class RepositoryDecorator : IRepository
    {
        protected readonly IRepository Repository;

        protected RepositoryDecorator(IRepository repository)
        {
            Repository = repository;
        }
        
        public virtual async Task<TAggregate> GetByIdAsync<TAggregate>(int id) where TAggregate : AggregateRoot
        {
            return await Repository.GetByIdAsync<TAggregate>(id);
        }
        
        public virtual async Task SaveAsync<TAggregate>(TAggregate aggregate) where TAggregate : AggregateRoot
        {
            await Repository.SaveAsync(aggregate);
        }

    }
}
