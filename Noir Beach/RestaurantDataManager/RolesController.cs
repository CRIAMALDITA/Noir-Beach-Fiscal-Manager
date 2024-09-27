using RestaurantData.TablesDataClasses;
using RestaurantDataManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager
{
    public class RolesController : DataController<RoleData>
    {
        RoleRepository roleRepository;
        public RolesController(QueryDataContext context) : base(context)
        {
            roleRepository = new RoleRepository(context);
            Repository = roleRepository;
        }

        public async override Task<bool> InitialCheck()
        {
            var listEmpty = !await DataIsNotEmpty().ConfigureAwait(false);

            if (listEmpty)
            {
                var firstUserCreated = await AddAsync(new RoleData()
                {
                    RolName = "ADMINISTRATOR",
                    CreationDate = DateTime.Now

                }).ConfigureAwait(false);

                return firstUserCreated;
            }
            return true;
        }
    }
}
