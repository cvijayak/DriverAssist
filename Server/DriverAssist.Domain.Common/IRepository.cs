using DriverAssist.Domain.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DriverAssist.Domain.Common
{
    public interface IRepository<T> where T : IEntity<Guid>
    {
        Task AddAsync(T item, CancellationToken cancellationToken);
        Task AddAsync(IEnumerable<T> items, CancellationToken cancellationToken);
        Task<T> UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
        Task<T> GetAsync(Guid id, CancellationToken cancellationToken);
        Task<(List<T> Items, long Total)> GetAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    }
}
