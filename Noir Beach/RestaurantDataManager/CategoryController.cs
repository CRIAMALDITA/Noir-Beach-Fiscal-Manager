using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager.Repositories;

namespace RestaurantDataManager
{
    public class CategoryController : DataController<CategoryData>
    {
        public CategoryRepository CategoryRepository;

        public CategoryController(QueryDataContext context) : base(context)
        {
            CategoryRepository = new(context);
            Repository = CategoryRepository;
        }

        public override async Task<List<CategoryData>> GetData()
        {
            return await GetAllAsync().ConfigureAwait(false);
        }

        public async override Task<bool> InitialCheck()
        {
            return true;
        }
    }
}
