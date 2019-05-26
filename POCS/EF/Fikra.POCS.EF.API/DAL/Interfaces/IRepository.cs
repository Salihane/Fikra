using System;
using System.Threading.Tasks;
using Fikra.POCS.EF.API.Model.Interfaces;

namespace Fikra.POCS.EF.API.DAL.Interfaces
{
    public interface IRepository<T, K> : IReadonlyRepository<T, K> where T : IEntity<K> where K : IEquatable<K>
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);

        Task<bool> SaveChangesAsync();
    }
}
