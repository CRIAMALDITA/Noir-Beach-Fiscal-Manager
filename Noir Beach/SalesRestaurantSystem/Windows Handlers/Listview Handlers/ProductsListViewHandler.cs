using RestaurantData.TablesDataClasses;
using RestaurantDataManager;
using SalesRestaurantSystem.WindowsHandlers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SalesRestaurantSystem
{
    internal class ProductsListViewHandler : ListViewPanelController<ProductData>
    {
        public ProductsListViewHandler(Window win) : base(win) { }

        public override void RemoveItemToList(ProductData value)
        {
            var element = Items.First(x => x.IdProduct == value.IdProduct);
            Items.Remove(element);
        }
        public override DataTable CreateDataTableFromGeneric(List<ProductData> items)
        {
            var table = new DataTable();

            table.Columns.Add("IdProduct", typeof(int));
            table.Columns.Add("Code", typeof(string));
            table.Columns.Add("ProductName", typeof(string));
            table.Columns.Add("Category", typeof(string));
            table.Columns.Add("Stock", typeof(string));
            table.Columns.Add("PurchasePrice", typeof(string));
            table.Columns.Add("SellPrice", typeof(string));
            table.Columns.Add("ProductState", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            foreach (var item in items)
            {
                var row = table.NewRow();
                row["IdProduct"] = item.IdProduct;
                row["Code"] = item.Code;
                row["ProductName"] = item.ProductName;
                row["Category"] = DataManager.Instance.Category.GetByIdAsync(item.IdCategory).Result.CategoryName;
                row["Stock"] = item.Stock;
                row["PurchasePrice"] = item.PurchasePrice;
                row["SellPrice"] = item.SellPrice;
                row["ProductState"] = item.ProductState ? "Active" : "Inactive";
                row["Date"] = item.CreationDate;
                table.Rows.Add(row);
            }

            return table;
        }
        public override ProductData CreateGenericFromDataTable(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                throw new ArgumentException("The provided DataTable is null or empty.");
            var row = table.Rows[0];
            var userData = DataManager.Instance.Product.GetByIdAsync(Convert.ToInt32(row["IdProduct"])).Result;
            return userData;
        }

        public override void Search(string type, string filter)
        {
            List<ProductData> list = new List<ProductData>();
            list = Items.ToList();
            if(filter != "")
            {
                foreach (var item in Items.ToList())
                {
                    switch (type)
                    {
                        case "ID": if (!item.IdCategory.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Code": if (!item.Code.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Name": if (!item.ProductName.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Category": if (!item.CategoryData.CategoryName.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "State": if (!((item.ProductState ? "Active" : "Inactive").ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", "")))) list.Remove(item); break;                   
                        case "CreationDate": if (!item.CreationDate.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                    }
                }
            }
            SearchedItems.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                int index = i;
                SearchedItems.Add(index);
            };
            RefreshListView();
        }
    }
}
