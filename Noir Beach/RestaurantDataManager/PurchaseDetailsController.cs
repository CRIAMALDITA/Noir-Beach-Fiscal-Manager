using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager.Repositories;

namespace RestaurantDataManager
{
    public class PurchaseDetailsController : DataController<PurchaseDetailsData>
    {
        public PurchaseDetailsRepository PurchasesDetailsRepository;

        public PurchaseDetailsController(QueryDataContext context) : base(context)
        {
            PurchasesDetailsRepository = new(context);
            Repository = PurchasesDetailsRepository;
        }

        public override async Task<List<PurchaseDetailsData>> GetData()
        {
            return await GetAllAsync().ConfigureAwait(false);
        }

        public async override Task<bool> InitialCheck()
        {
            return true;
        }
    }
}
