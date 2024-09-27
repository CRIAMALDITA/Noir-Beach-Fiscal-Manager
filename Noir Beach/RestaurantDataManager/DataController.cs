using Microsoft.EntityFrameworkCore;
using RestaurantData;
using RestaurantDataManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager
{
    public abstract class DataController<T> where T : class
    {
        protected QueryDataContext dataContext;
        protected IRepository<T> Repository { get; set; }
        private int maxEntitiesAllowed = 20;

        protected DataController(QueryDataContext context)
        {
            dataContext = context;
        }

        #region Data Managment Methods
        public async virtual Task<bool> InitialCheck()
        {
            return await Task.FromResult(true);
        }
        public async Task<bool> DataIsNotEmpty()
        {
            var data = await GetAllAsync().ConfigureAwait(false);
            return data.Count() > 0;
        }
        public virtual async Task<List<T>> GetData()
        {
            return await GetAllAsync().ConfigureAwait(false);
        }
        public async Task<bool> SetData(T data)
        {
            bool setted = false;

            if (await ExistEntityAsync(data))
            {
                await UpdateAsync(data).ConfigureAwait(false);
                setted = true;
            }
            else
            {
                await AddAsync(data).ConfigureAwait(false);
                setted = true;
            }
            return setted;
        }
       /* public async Task<List<T>> SearchByKeywordAsync(string keyword)
        {
            var dbSet = await dataContext.ExecuteQueriesAsync(x => x.Set<T>()).ConfigureAwait(false);

            var primitiveProperties = typeof(T).GetProperties()
                                                   .Where(p => p.PropertyType == typeof(string) ||
                                                               p.PropertyType == typeof(int) ||
                                                               p.PropertyType == typeof(float) ||
                                                               p.PropertyType == typeof(bool));

            IQueryable<T> query = dbSet;

            foreach (var property in primitiveProperties)
            {
                query = query.Where(e => property.PropertyType == typeof(string)
                                        ? EF.Functions.Like((string)property.GetValue(e), $"%{keyword}%")
                                        : property.GetValue(e).ToString().Contains(keyword));
            }

            return await query.ToListAsync().ConfigureAwait(false);
        }*/
        public async Task<List<T>> SearchByFieldInPrimitiveFieldsAsync(string fieldName, string keyword)
        {
            var result = await dataContext.ExecuteQueriesAsync(async x => x.Set<T>()).ConfigureAwait(false);
            var dbSet = result.Result;

            var property = typeof(T).GetProperty(fieldName);

            if (property == null || !(property.PropertyType == typeof(string) ||
                                      property.PropertyType == typeof(int) ||
                                      property.PropertyType == typeof(float) ||
                                      property.PropertyType == typeof(bool)))
            {
                throw new ArgumentException($"'{fieldName}' Does not exist");
            }
            var query = property.PropertyType == typeof(string)
                        ? dbSet.Where(e => EF.Functions.Like((string)property.GetValue(e), $"%{keyword}%"))
                        : dbSet.Where(e => property.GetValue(e).ToString().Contains(keyword));

            var list = await query.ToListAsync().ConfigureAwait(false);
            dataContext.ReleaseContext(result.Context);
            return list;
        }
        public Type GetGenericType(){ return typeof(T); }

        #endregion

        #region Interface Methods
        //Interface Methods
        public async Task<List<T>> GetAllAsync() { return await Repository.GetAllAsync().ConfigureAwait(false); }
        public async Task<T> GetByIdAsync(int id){ return await Repository.GetByIdAsync(id).ConfigureAwait(false); }
        public async Task<T> GetByNameAsync(string name){ return await Repository.GetByNameAsync(name).ConfigureAwait(false); }
        public async Task<bool> ExistEntityAsync(T entity){ return await Repository.ExistEntityAsync(entity).ConfigureAwait(false); }
        public async Task<bool> AddAsync(T entity){ return await Repository.AddAsync(entity).ConfigureAwait(false); }
        public async Task<bool> UpdateAsync(T entity){ return await Repository.UpdateAsync(entity).ConfigureAwait(false); }
        public async Task<bool> DeleteAsync(int id){ return await Repository.DeleteAsync(id).ConfigureAwait(false); }
        public async Task<T> AnyAsync(Func<T, bool> predicate) { return await Repository.AnyAsync(predicate).ConfigureAwait(false); }
        #endregion
    }
}
