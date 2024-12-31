using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(long id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(long id);

        Task<(IEnumerable<T>, int)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<T, bool>>? filter = null);
    }
}
