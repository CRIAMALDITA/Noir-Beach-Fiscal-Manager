using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager.Repositories;

namespace RestaurantDataManager
{
    public class SellController : DataController<SellData>
    {
        public SellRepository SellRepository;

        public SellController(QueryDataContext context) : base(context)
        {
            SellRepository = new(context);
            Repository = SellRepository;
        }

        public override async Task<List<SellData>> GetData()
        {
            return await GetAllAsync().ConfigureAwait(false);
        }

        public async override Task<bool> InitialCheck()
        {
            return true;
        }
    }
}
