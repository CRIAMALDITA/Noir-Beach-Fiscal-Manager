using Microsoft.EntityFrameworkCore;
using RestaurantData.TablesDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager.Repositories
{
    internal class PermissionRepository : Repository<PermissionData>
    {
        public PermissionRepository(QueryDataContext context) : base(context) { }


        public async override Task<List<PermissionData>> GetAllAsync()
        {
            var result = await _context.ExecuteQueriesAsync(async dbContext => dbContext.Set<PermissionData>().AsNoTracking());
            var list = result.Result.ToList();
            _context.ReleaseContext(result.Context);
            return list;
        }

        public async override Task<PermissionData> GetByIdAsync(int id)
        {
            var elements = await GetAllAsync().ConfigureAwait(false);
            return elements.FirstOrDefault(u => u.IdPermission == id);
        }
    }
}
