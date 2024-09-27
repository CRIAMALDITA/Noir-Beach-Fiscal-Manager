using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using RestaurantData.TablesDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected readonly QueryDataContext _context;

        public Repository(QueryDataContext context)
        {
            _context = context;
        }

        public abstract Task<List<T>> GetAllAsync();
        public abstract Task<T> GetByIdAsync(int id);

        public async Task<T> GetByNameAsync(string name)
        {
            var property = typeof(T).GetProperties()
                            .FirstOrDefault(p => p.Name.Contains("name", StringComparison.OrdinalIgnoreCase));

            if (property == null)
            {
                throw new InvalidOperationException($"No property containing 'name' found in type {typeof(T).Name}.");
            }

            try
            {
                var entities = await GetAllAsync().ConfigureAwait(false);
                return entities.FirstOrDefault(e => property.GetValue(e)?.ToString() == name);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error while trying to find entity by name: {name}", ex);
            }
        }

        public async Task<bool> ExistEntityAsync(T entity)
        {
            try
            {
                var dbSet = await GetAllAsync().ConfigureAwait(false);
                var query = dbSet.AsQueryable();

                foreach (var property in typeof(T).GetProperties())
                {
                    var value = property.GetValue(entity);
                    if (value != null)
                    {
                        query = query.Where(e => property.GetValue(e).Equals(value));
                    }
                }

                var result = await query.FirstOrDefaultAsync().ConfigureAwait(false);
                return result != null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while trying to check if entity exists", ex);
            }
        }

        public async Task<bool> AddAsync(T entity)
        {
            try
            {
                var result = await _context.ExecuteQueriesAsync(async x => x.Set<T>().Add(entity));
                _context.ReleaseContext(result.Context);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            try
            {
                await _context.ExecuteQueriesAsync(async x => 
                {
                    var result = x.Set<T>();
                    result.AsNoTracking();
                    return result.Update(entity);
                }, true);
                await _context.ExecuteQueriesAsync(async x => x.SaveChangesAsync()).ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var entity = await GetByIdAsync(id).ConfigureAwait(false);
                if (entity != null)
                {
                    await _context.ExecuteQueriesAsync(async x => x.Set<T>().Remove(entity));
                    await _context.ExecuteQueriesAsync(async x => x.SaveChangesAsync()).ConfigureAwait(false);
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<T> AnyAsync(Func<T, bool> predicate)
        {
            try
            {
                List<T> list = await GetAllAsync().ConfigureAwait(false);
                return list.Where(predicate).First();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error while trying to check if any entity matches the predicate", ex);
            }
        }
    }
}
