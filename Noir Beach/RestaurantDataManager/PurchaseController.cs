using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager.Repositories;

namespace RestaurantDataManager
{
    public class PurchaseController : DataController<PurchaseData>
    {
        public PurchaseRepository PurchaseRepository;

        public PurchaseController(QueryDataContext context) : base(context)
        {
            PurchaseRepository = new(context);
            Repository = PurchaseRepository;
        }

        public override async Task<List<PurchaseData>> GetData()
        {
            return await GetAllAsync().ConfigureAwait(false);
        }

        public async override Task<bool> InitialCheck()
        {
            return true;
        }
    }
}
