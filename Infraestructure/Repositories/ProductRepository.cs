using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace Infraestructure.Repositories
{
    public class ProductRepository : IRepository<Product>
    {
        private readonly DataContext _dataContext;

        public ProductRepository(DataContext dataContext)
        { 
            _dataContext = dataContext;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _dataContext.Product.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(long id)
        {
            return await _dataContext.Product.FindAsync(id);
        }

        public async Task AddAsync(Product product)
        {
            _dataContext.Product.Add(product);
            await _dataContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Product product)
        {
            _dataContext.Product.Update(product);
            await _dataContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(long id)
        {
            var product = await GetByIdAsync(id);
            if (product != null)
            {
                _dataContext.Product.Remove(product);
                await _dataContext.SaveChangesAsync();
            }
        }

        public async Task<(IEnumerable<Product>, int)> GetPagedAsync(int pageNumber, int pageSize, Expression<Func<Product, bool>>? filter = null)
        {
            var query = _dataContext.Product.AsQueryable();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalItems);
        }
    }
}
