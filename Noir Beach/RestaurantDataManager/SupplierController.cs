using RestaurantData.TablesDataClasses;
using RestaurantDataManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager
{
    public class SupplierController : DataController<SupplierData>
    {
        public SupplierRepository SupplierRepository;
        public SupplierController(QueryDataContext context) : base(context)
        {
            SupplierRepository = new(context);
            Repository = SupplierRepository;
        }
    }
}
