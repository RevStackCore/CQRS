using System;
using System.Threading.Tasks;
using RevStackCore.CQRS.Domain;

namespace RevStackCore.CQRS.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<T> GetAsync<T>(int id, int? version = null) where T : AggregateRoot;
        void Add<T>(T aggregate) where T : AggregateRoot;
        Task CommitAsync();
    }
}
