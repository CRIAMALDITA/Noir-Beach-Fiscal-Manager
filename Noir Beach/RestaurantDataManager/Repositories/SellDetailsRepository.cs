using Microsoft.EntityFrameworkCore;
using RestaurantData.TablesDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager.Repositories
{
    public class SellDetailsRepository : Repository<SellDetailsData>
    {
        public SellDetailsRepository(QueryDataContext context) : base(context) {}

        public async override Task<List<SellDetailsData>> GetAllAsync()
        {
            var result = await _context.ExecuteQueriesAsync(async dbContext => dbContext.Set<SellDetailsData>().AsNoTracking().Include(p => p.Product).Include(s => s.Sell));
            var list = result.Result.ToList();
            _context.ReleaseContext(result.Context);
            return list;
        }

        public async override Task<SellDetailsData> GetByIdAsync(int id)
        {
            var elements = await GetAllAsync().ConfigureAwait(false);
            return elements.FirstOrDefault(u => u.IdSellDetails == id);
        }
    }
}
