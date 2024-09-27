using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager.Repositories;

namespace RestaurantDataManager
{
    public class ProductsController : DataController<ProductData>
    {
        public ProductRepository ProductRepository;

        public ProductsController(QueryDataContext context) : base(context)
        {
            ProductRepository = new(context);
            Repository = ProductRepository;
        }

        public override async Task<List<ProductData>> GetData()
        {
            return await GetAllAsync().ConfigureAwait(false);
        }

        public async override Task<bool> InitialCheck()
        {
            return true;
        }
    }
}
