using DriverAssist.Domain.Common;
using DriverAssist.Domain.Common.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.Domain.MongoDB
{

    internal abstract class MongoRepositoryBase<T> : IRepository<T> where T : IEntity<Guid>
    {
        protected readonly IMongoDbStorage _dbStorage;

        protected MongoRepositoryBase(IMongoDbStorage dbStorage)
        {
            _dbStorage = dbStorage;
        }

        protected abstract IMongoCollection<T> Collection { get; }

        public async Task AddAsync(T item, CancellationToken cancellationToken)
        {
            await Collection.InsertOneAsync(item, cancellationToken: cancellationToken);
        }

        public async Task AddAsync(IEnumerable<T> items, CancellationToken cancellationToken)
        {
            await Collection.InsertManyAsync(items, cancellationToken: cancellationToken);
        }

        public async Task<T> UpdateAsync(T entity, CancellationToken cancellationToken)
        {
            return await Collection
                .FindOneAndReplaceAsync(x => x.Id == entity.Id, entity, cancellationToken: cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            await Collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await Collection
                .AsQueryable()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
