using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RestaurantData;
using RestaurantData.TablesDataClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantDataManager
{
    public class DataManager
    {
        public static DataManager Instance = new DataManager();
        private readonly QueryDataContext dataContext = new();

        //Managers
        public UserController User;
        public RolesController Role;
        public PermissionController Permission;
        public CategoryController Category;
        public ProductsController Product;
        public ClientController Client;
        public SellDetailsController SellDetails;
        public SellController Sell;
        public BussinessController Bussiness;
        public SupplierController Supplier;

        public DataManager()
        {
            User = new(dataContext);
            Role = new(dataContext);
            Permission = new(dataContext);
            Category = new(dataContext);
            Product = new(dataContext);
            Client = new(dataContext);
            SellDetails = new(dataContext);
            Sell = new(dataContext);
            Bussiness = new();
            Supplier = new(dataContext);
        }

        public async Task<string> CheckConnection()
        {
            try
            {
                await User.GetData().ConfigureAwait(false);
                return "I_Base Data Connection Successfully!";
            }
            catch (SqlException ex)
            {
                return $"E_SQL Base Data Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"E_Connection Failed: {ex.Message}";
            }
        }

        public async Task<bool> InitialCheck()
        {
            if (!await Permission.InitialCheck().ConfigureAwait(false)) return false;
            if (!await Role.InitialCheck().ConfigureAwait(false)) return false;
            User.ADMIN_PERMISSION_ID = await Permission.GetByNameAsync("ADMINISTRATOR").ConfigureAwait(false);
            User.ADMIN_ROLE_ID = await Role.GetByNameAsync("ADMINISTRATOR").ConfigureAwait(false);
            if (!await User.InitialCheck().ConfigureAwait(false)) return false;
            return true;
        }

        public DataController<T> GenericController<T>() where T : class
        {
            if (typeof(T) == User.GetGenericType()) return User as DataController<T>;
            if (typeof(T) == Permission.GetGenericType()) return Permission as DataController<T>;
            if (typeof(T) == Role.GetGenericType()) return Role as DataController<T>;
            if (typeof(T) == Category.GetGenericType()) return Category as DataController<T>;
            if (typeof(T) == Product.GetGenericType()) return Product as DataController<T>;
            if (typeof(T) == Client.GetGenericType()) return Client as DataController<T>;
            if (typeof(T) == Sell.GetGenericType()) return Sell as DataController<T>;
            if (typeof(T) == SellDetails.GetGenericType()) return SellDetails as DataController<T>;
            return null;
        }


        public string[] TypeParametersToString<T>()
        {
            var parameters = typeof(T).GetProperties();
            string[] result = new string[parameters.Length];
            for(int i = 0; i < result.Length; i++) result[i] = parameters[i].Name;
            return result;
        }
    }
}
