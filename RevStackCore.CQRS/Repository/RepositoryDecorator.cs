using System;
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

        public virtual TAggregate GetById<TAggregate>(Guid id) where TAggregate : AggregateBase
        {
            return Repository.GetById<TAggregate>(id);
        }

        public virtual async Task<TAggregate> GetByIdAsync<TAggregate>(Guid id) where TAggregate : AggregateBase
        {
            return await Repository.GetByIdAsync<TAggregate>(id);
        }

        public virtual void Save<TAggregate>(TAggregate aggregate) where TAggregate : AggregateBase
        {
            Repository.Save(aggregate);
        }

        public virtual async Task SaveAsync<TAggregate>(TAggregate aggregate) where TAggregate : AggregateBase
        {
            await Repository.SaveAsync(aggregate);
        }

    }
}
