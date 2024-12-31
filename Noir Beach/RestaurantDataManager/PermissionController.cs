using RestaurantData.TablesDataClasses;
using RestaurantDataManager.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager
{
    public class PermissionController : DataController<PermissionData>
    {
        private PermissionRepository permissionRepository;
        public PermissionController(QueryDataContext context) : base(context)
        {
            permissionRepository = new PermissionRepository(context);
            Repository = permissionRepository;
        }

        public async override Task<bool> InitialCheck()
        {
            var listEmpty = !await DataIsNotEmpty().ConfigureAwait(false);

            if (listEmpty)
            {
                var firstUserCreated = await AddAsync(new PermissionData()
                {
                    PermissionName = "ADMINISTRATOR",
                    CreationDate = DateTime.Now

                }).ConfigureAwait(false);
                await AddAsync(new PermissionData()
                {
                    PermissionName = "Owner",
                    CreationDate = DateTime.Now

                }).ConfigureAwait(false);
                await AddAsync(new PermissionData()
                {
                    PermissionName = "Standar",
                    CreationDate = DateTime.Now

                }).ConfigureAwait(false);


                return firstUserCreated;
            }
            return true;
        }
    }
}
