using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestaurantData.TablesDataClasses;
using RestaurantDataManager.Repositories;

namespace RestaurantDataManager
{
    public class UserController : DataController<UserData>
    {
        public UserData CurrentUserLogged;
        public RoleData ADMIN_ROLE_ID;
        public PermissionData ADMIN_PERMISSION_ID;
        public UserRepository UserRepository;

        public UserController(QueryDataContext context) : base(context)
        {
            UserRepository = new(context);
            Repository = UserRepository;
        }

        public async Task<UserData> LogInUser(string username, string password)
        {
            var users = await GetData().ConfigureAwait(false);
            users = users.Where(u => u.Document == username && u.UserPassword == password).ToList();
            CurrentUserLogged = users.Count > 0 ? users.First() : null;
            return CurrentUserLogged;
        }

        public override async Task<List<UserData>> GetData()
        {
            return await GetAllAsync().ConfigureAwait(false);
        }

        public async override Task<bool> InitialCheck()
        {
            var listEmpty = !await DataIsNotEmpty().ConfigureAwait(false);

            if (listEmpty)
            {
                var _user = new UserData()
                {
                    Document = "admin",
                    UserPassword = "custadmin",
                    IdRol = ADMIN_ROLE_ID.IdRol,
                    IdPermission = ADMIN_PERMISSION_ID.IdPermission,
                    CreationDate = DateTime.Now,
                    UserState = true,
                };
                var firstUserCreated = await AddAsync(_user);
                return firstUserCreated;
            }
            return true;
        }
    }
}
