using Point_of_sale_for_Restaurant;
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
    internal class SellDetailsListViewHandler : ListViewPanelController<SellDetailsData>
    {
        public SellDetailsListViewHandler(Window win) : base(win) { }

        public override void RemoveItemToList(SellDetailsData value)
        {
            var element = Items.First(x => x.IdSellDetails == value.IdSellDetails);
            Items.Remove(element);
        }
        public override DataTable CreateDataTableFromGeneric(List<SellDetailsData> items)
        {
            var table = new DataTable();

            table.Columns.Add("IdSellDetails", typeof(int));
            table.Columns.Add("Code", typeof(int));
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("SellPrice", typeof(string));
            table.Columns.Add("Amount", typeof(string));
            table.Columns.Add("Subtotal", typeof(string));

            foreach (var item in items)
            {
                ProductData reference = DataManager.Instance.Product.GetByIdAsync(item.IdProduct).Result;
                var row = table.NewRow();
                row["IdSellDetails"] = item.IdSellDetails;
                row["Code"] = reference.Code;
                row["Product"] = reference.ProductName;
                row["SellPrice"] = reference.SellPrice;
                row["Amount"] = item.ProductsCount;
                row["Subtotal"] = item.SubTotal;
                table.Rows.Add(row);
            }

            return table;
        }
        public override SellDetailsData CreateGenericFromDataTable(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                throw new ArgumentException("The provided DataTable is null or empty.");
            var row = table.Rows[0];
            var userData = DataManager.Instance.SellDetails.GetByIdAsync(Convert.ToInt32(row["IdSellDetails"])).Result;
            return userData;
        }

        public override void Search(string type, string filter)
        {
           
        }

        public override void DobleClick()
        {
        
        }
    }
}
