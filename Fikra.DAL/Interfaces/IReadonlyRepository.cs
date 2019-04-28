using Fikra.Model;
using Fikra.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Fikra.DAL.Interfaces
{
    public interface IReadonlyRepository<T, K> where T : IEntity<K> where K : IEquatable<K>
    {
        IQueryable<T> GetAll();
        Task<T> GetByIdAsync(K id);
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
        Task<long> CountAsync();
    }
}
