using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fikra.POCS.EF.API.DAL.Interfaces;
using Fikra.POCS.EF.API.Model.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fikra.POCS.EF.API.DAL
{
    public class FikraRepository<T, K> : IRepository<T, K> where T : class, IEntity<K> where K : IEquatable<K>
    {
        private readonly FikraContext _context;
        private DbSet<T> _dbSet;

        public FikraRepository(FikraContext context)
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

        public async Task<long> CountAsync()
        {
            return await _dbSet.LongCountAsync();
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
        public async Task<IQueryable<T>> SearchForAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
        {
            var query = await SearchForAsync(predicate);
            return await Task.Run(() => 
            includes.Aggregate(query, (current, includeProperty) => current.Include(includeProperty)));
        }
    }
}
