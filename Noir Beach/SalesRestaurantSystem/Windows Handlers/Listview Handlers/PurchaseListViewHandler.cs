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
    internal class PUrchaseListViewHandler : ListViewPanelController<PurchaseData>
    {
        public PUrchaseListViewHandler(Window win) : base(win) { }

        public override void RemoveItemToList(PurchaseData value)
        {
            var element = Items.First(x => x.IdPurchase == value.IdPurchase);
            Items.Remove(element);
        }
        public override DataTable CreateDataTableFromGeneric(List<PurchaseData> items)
        {
            var table = new DataTable();

            table.Columns.Add("IdPurchase", typeof(int));
            table.Columns.Add("UserName", typeof(string));
            table.Columns.Add("Supplier", typeof(string));
            table.Columns.Add("IdType", typeof(string));
            table.Columns.Add("IdNum", typeof(string));
            table.Columns.Add("Total", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            foreach (var item in items)
            {
                var row = table.NewRow();
                row["IdPurchase"] = item.IdPurchase;
                row["UserName"] = item.User.FullName;
                row["Supplier"] = item.Supplier.FiscalName;
                row["IdType"] = item.IdentificationType;
                row["IdNum"] = item.IdentificationNumber;
                row["Total"] = item.Total;
                row["Date"] = item.CreationDate;
                table.Rows.Add(row);
            }

            return table;
        }
        public override PurchaseData CreateGenericFromDataTable(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                throw new ArgumentException("The provided DataTable is null or empty.");
            var row = table.Rows[0];
            var purchaseData = DataManager.Instance.Purchase.GetByIdAsync(Convert.ToInt32(row["IdPurchase"])).Result;
            return purchaseData;
        }

        public override void Search(string type, string filter)
        {
            List<PurchaseData> list = new List<PurchaseData>();
            list = Items.ToList();
            if (filter != "")
            {
                FilterSetted = false;
                foreach (var item in Items.ToList())
                {
                    switch (type)
                    {
                        case "ID": if (!item.IdPurchase.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "User": if (!item.User.FullName.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Supplier":    if (!item.Supplier.FiscalName.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "ID Num": if (!item.IdentificationNumber.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Total": if (!item.Total.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "CreationDate": if (!item.CreationDate.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                    }
                }
            }
            SearchedItems.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                int index = Items.IndexOf(list[i]);
                SearchedItems.Add(index);
            };
            if (SearchedItems.Count > 0) FilterSetted = true;
            RefreshListView();
        }
    }
}
