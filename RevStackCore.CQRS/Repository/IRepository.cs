using System;
using System.Threading.Tasks;
using RevStackCore.CQRS.Domain;


namespace RevStackCore.CQRS.Repository
{
    public interface IRepository
    {
        //T GetById<T>(int id) where T : AggregateBase;
        Task<T> GetByIdAsync<T>(int id) where T : AggregateRoot;
        //void Save<T>(T aggregate) where T : AggregateBase;
        Task SaveAsync<T>(T aggregate) where T : AggregateRoot;
    }
}
