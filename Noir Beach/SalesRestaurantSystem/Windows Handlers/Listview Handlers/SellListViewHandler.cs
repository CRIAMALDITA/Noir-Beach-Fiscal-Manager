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
    internal class SellListViewHandler : ListViewPanelController<SellData>
    {
        public SellListViewHandler(Window win) : base(win) { }

        public override void RemoveItemToList(SellData value)
        {
            var element = Items.First(x => x.IdSell == value.IdSell);
            Items.Remove(element);
        }
        public override DataTable CreateDataTableFromGeneric(List<SellData> items)
        {
            var table = new DataTable();

            table.Columns.Add("IdSell", typeof(int));
            table.Columns.Add("UserName", typeof(string));
            table.Columns.Add("IdType", typeof(string));
            table.Columns.Add("IdNum", typeof(string));
            table.Columns.Add("ClientId", typeof(string));
            table.Columns.Add("ClientName", typeof(string));
            table.Columns.Add("Subtotal", typeof(string));
            table.Columns.Add("Exchange", typeof(string));
            table.Columns.Add("InvoiceNumber", typeof(int));
            table.Columns.Add("Total", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            foreach (var item in items)
            {
                var row = table.NewRow();
                row["IdSell"] = item.IdSell;
                row["UserName"] = item.User.FullName;
                row["IdType"] = item.IdentificationType;
                row["IdNum"] = item.IdentificationNumber;
                row["ClientId"] = item.ClientIdentification;
                row["ClientName"] = item.ClientName;
                row["Subtotal"] = item.SubTotal;
                row["Exchange"] = item.Exchange;
                row["InvoiceNumber"] = item.InvoiceNumber;
                row["Total"] = item.Total.ToString();
                row["Date"] = item.CreationDate.ToString();
                table.Rows.Add(row);
            }

            return table;
        }
        public override SellData CreateGenericFromDataTable(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                throw new ArgumentException("The provided DataTable is null or empty.");
            var row = table.Rows[0];
            var SellData = DataManager.Instance.Sell.GetByIdAsync(Convert.ToInt32(row["IdSell"])).Result;
            return SellData;
        }

        public override void Search(string type, string filter)
        {
            List<SellData> list = new List<SellData>();
            list = Items.ToList();
            if (filter != "")
            {
                FilterSetted = false;
                foreach (var item in Items.ToList())
                {
                    switch (type)
                    {
                        case "IdSell": if (!item.IdSell.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "IdNum": if (!item.IdentificationNumber.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "UserName": if (!item.User.FullName.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "ClientId": if (!item.ClientIdentification.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "ClientName": if (!item.ClientName.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Total": if (!item.Total.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "InvoiceNumber": if (!item.InvoiceNumber.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
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

        public override void DobleClick()
        {
            SaleDetailsWindow win = new SaleDetailsWindow(Items[_listView.SelectedIndex]);
            win.Show();
        }
    }
}
