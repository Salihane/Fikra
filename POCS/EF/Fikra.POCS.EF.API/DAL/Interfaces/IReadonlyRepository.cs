using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fikra.POCS.EF.API.Model.Interfaces;

namespace Fikra.POCS.EF.API.DAL.Interfaces
{
    public interface IReadonlyRepository<T, K> where T : IEntity<K> where K : IEquatable<K>
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(K id);
        Task<IQueryable<T>> SearchForAsync(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> SearchForAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
        Task<long> CountAsync();
    }
}
