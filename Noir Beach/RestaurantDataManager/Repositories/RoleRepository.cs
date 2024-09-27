using Microsoft.EntityFrameworkCore;
using RestaurantData.TablesDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager.Repositories
{
    public class RoleRepository : Repository<RoleData>
    {
        public RoleRepository(QueryDataContext context) : base(context) { }

        public async override Task<List<RoleData>> GetAllAsync()
        {
            var result = await _context.ExecuteQueriesAsync(async dbContext => dbContext.Set<RoleData>().AsNoTracking());
            var list = result.Result.ToList();
            _context.ReleaseContext(result.Context);
            return list;
        }

        public async override Task<RoleData> GetByIdAsync(int id)
        {
            var elements = await GetAllAsync().ConfigureAwait(false);
            return elements.FirstOrDefault(u => u.IdRol == id);
        }
    }
}
