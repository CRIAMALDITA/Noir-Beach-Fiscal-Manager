using Microsoft.EntityFrameworkCore;
using RestaurantData.TablesDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager.Repositories
{
    public class UserRepository : Repository<UserData>
    {
        public UserRepository(QueryDataContext context) : base(context) {}

        public async override Task<List<UserData>> GetAllAsync()
        {
            try
            {
                var result = await _context.ExecuteQueriesAsync(async dbContext => dbContext.Set<UserData>().AsNoTracking().Include(u => u.RoleData).Include(u => u.PermissionData));
                var list = result.Result.ToList();
                _context.ReleaseContext(result.Context);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async override Task<UserData> GetByIdAsync(int id)
        {
            var elements = await GetAllAsync().ConfigureAwait(false);
            return elements.FirstOrDefault(u => u.IdUser == id);
        }
    }
}
