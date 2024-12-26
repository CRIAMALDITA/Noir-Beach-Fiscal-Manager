using Microsoft.EntityFrameworkCore;
using RestaurantData.TablesDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager.Repositories
{
    public class PurchaseDetailsRepository : Repository<PurchaseDetailsData>
    {
        public PurchaseDetailsRepository(QueryDataContext context) : base(context) {}

        public async override Task<List<PurchaseDetailsData>> GetAllAsync()
        {
            var result = await _context.ExecuteQueriesAsync(async dbContext => dbContext.Set<PurchaseDetailsData>().AsNoTracking().Include(p => p.Product).Include(s => s.Purchase));
            var list = result.Result.ToList();
            _context.ReleaseContext(result.Context);
            return list;
        }

        public async override Task<PurchaseDetailsData> GetByIdAsync(int id)
        {
            var elements = await GetAllAsync().ConfigureAwait(false);
            return elements.FirstOrDefault(u => u.IdPurchaseDetails == id);
        }
    }
}
