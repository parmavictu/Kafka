using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kafka.Domain.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        Task AddASync(TEntity obj);
        void Delete(Expression<Func<TEntity, bool>> predicate);
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<int> SaveChangesAsync();
        void Update(Expression<Func<TEntity, bool>> expression, TEntity obj);
        Task UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity obj);
    }
}