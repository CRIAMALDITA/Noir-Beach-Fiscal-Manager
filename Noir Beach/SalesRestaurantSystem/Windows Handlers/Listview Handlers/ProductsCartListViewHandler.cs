using RestaurantData.TablesDataClasses;
using RestaurantDataManager;
using SalesRestaurantSystem.WindowsHandlers.ListviewHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SalesRestaurantSystem.WindowsHandlers.Listview_Handlers
{
    public class ProductsCartListViewHandler : CartListViewHandler<ProductData>
    {
        public ProductsCartListViewHandler(Window win) : base(win){}

        public override List<ProductData> CreateGenericsFormCartElements()
        {
            List<ProductData> db = DataManager.Instance.Product.GetAllAsync().Result;
            List<ProductData> currentData = db.Where(x => Items.Any(y => y.Name == x.ProductName)).ToList();
            return currentData;
        }
    }
}
