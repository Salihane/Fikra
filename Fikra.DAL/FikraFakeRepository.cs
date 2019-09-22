using Fikra.DAL.Interfaces;
using Fikra.Model.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fikra.Common.Extensions;
using Fikra.Common.Helpers;
using System.Linq.Dynamic.Core;
using Fikra.DAL.StoredProcedures;
using Fikra.Model.QueryEntities;
using Microsoft.EntityFrameworkCore.Storage;

namespace Fikra.DAL
{
    public class FikraFakeRepository<T, K> : IFakeRepository<T, K> where T : class, IEntity<K> where K : IEquatable<K>
	{
		private readonly FikraFakeContext _context;
		private DbSet<T> _dbSet;

		public FikraFakeRepository(FikraFakeContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}
		public void Add(T entity)
		{
			_dbSet.Add(entity);
		}

		public IQueryable<T> GetAll()
		{
			return _dbSet;
		}

		public async Task<PagedList<T>> SearchForAsync(Expression<Func<T, bool>> predicate,
			IResourceParameters<T, K> resourceParameters)
		{
			var collection = await SearchForAsync(predicate);

			//var applyFilter = resourceParameters?.ResourceFilter != null;
			//if (applyFilter)
			//{
			//	collection = collection
			//		.Where(resourceParameters.ResourceFilter.Expression);
			//}

			//var hasSearchQuery = !string.IsNullOrWhiteSpace(resourceParameters?.SearchQuery);
			//if (hasSearchQuery)
			//{
			//	collection = collection.SearchFor(resourceParameters.SearchQuery);
			//}

			var pageNumber = resourceParameters?.PageNumber ?? 1;
			var pageSize = resourceParameters?.PageSize ?? collection.Count();

			return PagedList<T>.Create(collection, pageNumber, pageSize);
		}

		public async Task<long> CountAsync()
		{
			return await _dbSet.LongCountAsync();
		}

		public async Task<Dictionary<string, int>> CountChildsAsync(T entity, params string[] childNames)
		{
			var childsCount = new Dictionary<string, int>();
			foreach (var childName in childNames)
			{
				var count = await CountChildAsync(entity, childName);
				childsCount.Add(childName, count);
			}

			return childsCount;
		}

		public async Task<int> CountChildAsync(T entity, string childName)
		{
			return await Task.Run(() => _context.Entry(entity)
				.Collection(childName)
				.Query()
				.Count());
		}

		public Task<IEnumerable<TQuery>> ExecuteStoredProc<TQuery>(IStoredProcedure storedProcedure) where TQuery : class
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<TQuery>> ExecuteStoredProc<TQuery>(string storedProcName, params object[] storedProcParams) where TQuery : class
		{
			throw new NotImplementedException();
		}

		public async Task<T> GetByIdAsync(K id)
		{
			return await _dbSet.FindAsync(id);
		}

		public void Remove(T entity)
		{
			_dbSet.Remove(entity);
		}

		public async Task<bool> SaveChangesAsync()
		{
			return (await _context.SaveChangesAsync()) > 0;
		}

		public Task<IDbContextTransaction> StartTransactionAsync()
		{
			throw new NotImplementedException();
		}

		public object ExecuteStoredProc(string storedProcName, params SqlParameter[] parameters)
		{
			throw new NotImplementedException();
		}

		public async Task<IQueryable<T>> SearchForAsync(Expression<Func<T, bool>> predicate)
		{
			return await Task.Run(() => _dbSet.Where(predicate));
		}

		public void Update(T entity)
		{
			_context.Entry(entity).State = EntityState.Modified;
		}

		// Solution for including childs found here: https://gist.github.com/oneillci/3205384
		// To include subchilds (not implemented yet) check here: https://github.com/digipolisantwerp/dataaccess_aspnetcore
		public async Task<IQueryable<T>> SearchForAsync(
			Expression<Func<T, bool>> predicate, 
			params Expression<Func<T, object>>[] includes)
		{
			var query = await SearchForAsync(predicate);
			return await Task.Run(() =>
				includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)));
		}
	}
}
