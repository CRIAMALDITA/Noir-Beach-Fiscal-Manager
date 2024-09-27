using Microsoft.EntityFrameworkCore;
using RestaurantData.TablesDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager.Repositories
{
    public class ClientRepository : Repository<ClientData>
    {
        public ClientRepository(QueryDataContext context) : base(context)
        {
        }

        public async override Task<List<ClientData>> GetAllAsync()
        {
            var result = await _context.ExecuteQueriesAsync(async dbContext => dbContext.Set<ClientData>().AsNoTracking());
            var list = result.Result.ToList();
            _context.ReleaseContext(result.Context);
            return list;
        }

        public async override Task<ClientData> GetByIdAsync(int id)
        {
            var elements = await GetAllAsync().ConfigureAwait(false);
            return elements.FirstOrDefault(u => u.IdClient == id);
        }
    }
}
