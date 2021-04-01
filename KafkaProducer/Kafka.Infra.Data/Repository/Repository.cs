using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Kafka.Domain.Interfaces;
using Kafka.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Kafka.Infra.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected readonly KafkaContext<TEntity> _context = null;

        public Repository()
        {
            _context = new KafkaContext<TEntity>();
        }

        #region Sync

        public void Add(TEntity obj)
        {
            _context.Collection.InsertOne(obj);
        }

        public void Update(Expression<Func<TEntity, bool>> expression, TEntity obj)
        {
            _context.Collection.ReplaceOne(expression, obj);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            _context.Collection.FindOneAndDelete(predicate);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _context.Collection.Find(_ => true).ToList();
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Collection.Find(predicate).ToList();
        }

        #endregion Sync

        #region Async

        public async Task AddASync(TEntity obj)
        {
            await _context.Collection.InsertOneAsync(obj);
        }

        public async Task UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity obj)
        {
            await _context.Collection.ReplaceOneAsync(expression, obj);
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            await _context.Collection.FindOneAndDeleteAsync(predicate);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return (await _context.Collection.FindAsync(predicate)).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return (await _context.Collection.FindAsync(_ => true)).ToList();
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        #endregion Async

        private ObjectId GetInternalId(string id)
        {
            ObjectId internalId;
            if (!ObjectId.TryParse(id, out internalId))
                internalId = ObjectId.Empty;

            return internalId;
        }

    }
}