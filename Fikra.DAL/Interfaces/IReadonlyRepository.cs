using Fikra.Model.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fikra.Common.Helpers;
using Fikra.DAL.StoredProcedures;
using Fikra.DAL.StoredProcedures.Interfaces;
using Fikra.Model.QueryEntities;

namespace Fikra.DAL.Interfaces
{
    public interface IReadonlyRepository<T, K> where T : IEntity<K> where K : IEquatable<K>
    {
		IQueryable<T> GetAll();
        Task<T> GetByIdAsync(K id);
        Task<IQueryable<T>> SearchForAsync(Expression<Func<T, bool>> predicate);
        Task<IQueryable<T>> SearchForAsync(Expression<Func<T, bool>> predicate,
			params Expression<Func<T, object>>[] includes);
		Task<PagedList<T>> SearchForAsync(Expression<Func<T, bool>> predicate,
			IResourceParameters<T, K> resourceParameters);
		Task<long> CountAsync();
		Task<Dictionary<string, int>> CountChildsAsync(T entity, params string[] childNames);

		Task<int> CountChildAsync(T entity, string childName);

		Task<IEnumerable<TQuery>> ExecuteStoredProc<TQuery>(IStoredProcedure storedProcedure)
			where TQuery : class;

    }
}
