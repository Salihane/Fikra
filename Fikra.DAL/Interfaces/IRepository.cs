using Fikra.Model;
using Fikra.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Fikra.Model.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using Task = Fikra.Model.Entities.Task;

namespace Fikra.DAL.Interfaces
{
    public interface IRepository<T, K> : IReadonlyRepository<T, K> where T : IEntity<K> where K : IEquatable<K>
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);

        Task<bool> SaveChangesAsync();
        Task<IDbContextTransaction> StartTransactionAsync();
        object ExecuteStoredProc(string storedProcName, params SqlParameter[] parameters);
    }
}
