using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using RestaurantData.TablesDataClasses;

namespace RestaurantDataManager
{

    public class QueryDataContext
    {
        DataContextManager manager = new DataContextManager();
        /*public T ExecuteQueries<T>(Func<DbContext, T> query, bool debug = false)
        {
            var context = manager.GetContext();
            if (debug)
            {
                try
                {
                    T? t = query.Invoke(context);
                    manager.ReleaseContext(context);
                    return t;
                }
                catch(DbUpdateException ex)
                {
                    throw;
                }
            }

            T result=  query.Invoke(context);
            manager.ReleaseContext(context).Wait();
            return result;
        }*/
        public async Task<QuerieResult<T>> ExecuteQueriesAsync<T>(Func<DbContext, Task<T>> query, bool debug = false) where T : class
        {
            var context = manager.GetContext();
            if (debug)
            {
                try
                {
                    await query.Invoke(context).ConfigureAwait(false);
                }
                catch (DbUpdateException ex)
                {
                    throw;
                }
            }
            T result = await query.Invoke(context).ConfigureAwait(false);
            try
            {
                await context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
            }
            return new QuerieResult<T>(context, result);
        }

        public void ReleaseContext(DataContext context)
        {
            manager.ReleaseContext(context);
        }

        public struct QuerieResult<T> where T : class
        {
            public DataContext Context;
            public T Result;

            public QuerieResult(DataContext db, T result)
            {
                Context = db;
                Result = result;
            }
        }

        private class DataContextManager
        {
            private List<DataContext> _availableContexts = new List<DataContext>();
            private readonly int _maxPoolSize = 10;

            public DataContextManager()
            {
                for(int i = 0; i < _maxPoolSize; i++)_availableContexts.Add(new DataContext());
            }


            public DataContext GetContext()
            {
                var context = _availableContexts.FirstOrDefault(x => x.inUse == false);
                if (context == null) throw new InvalidOperationException("No available contexts.");
                context.inUse = true;
                return context;
            }

            public void ReleaseContext(DataContext context)
            {
                context.inUse = false;
            }
        }
        public class DataContext : DbContext, IDisposable
        {
            public bool inUse;
            public DbSet<UserData> Users { get; set; }
            public DbSet<RoleData> Roles { get; set; }
            public DbSet<PermissionData> Permissions { get; set; }
            public DbSet<CategoryData> Category { get; set; }
            public DbSet<ProductData> Product { get; set; }
            public DbSet<ClientData> Client { get; set; }
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlServer("Data Source=ALISSS;Initial Catalog=BDSALES_RESTAURANT_SYSTEM;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False; MultipleActiveResultSets=True")
                    .EnableSensitiveDataLogging()  // Registra datos sensibles en los logs
                    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);  // Habilita un nivel de logging más detallado
            }
        }
    }
}
