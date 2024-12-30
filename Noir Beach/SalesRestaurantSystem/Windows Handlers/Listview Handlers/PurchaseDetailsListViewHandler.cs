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
    internal class PurchaseDetailsListViewHandler : ListViewPanelController<PurchaseDetailsData>
    {
        public PurchaseDetailsListViewHandler(Window win) : base(win) { }

        public override void RemoveItemToList(PurchaseDetailsData value)
        {
            var element = Items.First(x => x.IdPurchaseDetails == value.IdPurchaseDetails);
            Items.Remove(element);
        }
        public override DataTable CreateDataTableFromGeneric(List<PurchaseDetailsData> items)
        {
            var table = new DataTable();

            table.Columns.Add("IdPurchaseDetails", typeof(int));
            table.Columns.Add("Code", typeof(int));
            table.Columns.Add("Product", typeof(string));
            table.Columns.Add("Price", typeof(string));
            table.Columns.Add("Amount", typeof(string));
            table.Columns.Add("Subtotal", typeof(string));

            foreach (var item in items)
            {
                ProductData reference = DataManager.Instance.Product.GetByIdAsync(item.IdProduct).Result;
                var row = table.NewRow();
                row["IdPurchaseDetails"] = item.IdPurchaseDetails;
                row["Code"] = reference.Code;
                row["Product"] = reference.ProductName;
                row["Price"] = reference.PurchasePrice;
                row["Amount"] = item.ProductCount;
                row["Subtotal"] = item.SubTotal;
                table.Rows.Add(row);
            }

            return table;
        }
        public override PurchaseDetailsData CreateGenericFromDataTable(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                throw new ArgumentException("The provided DataTable is null or empty.");
            var row = table.Rows[0];
            var userData = DataManager.Instance.PurchaseDetails.GetByIdAsync(Convert.ToInt32(row["IdPurchaseDetails"])).Result;
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
