using RestaurantData.TablesDataClasses;
using RestaurantDataManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SalesRestaurantSystem
{
    internal class ClientsListViewHandler : ListViewPanelController<ClientData>
    {
        public ClientsListViewHandler(Window win) : base(win){}

        public override void RemoveItemToList(ClientData value)
        {
            var element = Items.First(x => x.IdClient == value.IdClient);
            Items.Remove(element);
        }
        public override DataTable CreateDataTableFromGeneric(List<ClientData> items)
        {
            var table = new DataTable();

            table.Columns.Add("IdClient", typeof(int));
            table.Columns.Add("Document", typeof(string));
            table.Columns.Add("FullName", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Tel", typeof(string));
            table.Columns.Add("Account", typeof(string));
            table.Columns.Add("Balance", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            foreach (var item in items)
            {
                var row = table.NewRow();
                row["IdClient"] = item.IdClient;
                row["Document"] = item.Document;
                row["FullName"] = item.FullName;
                row["Tel"] = item.Telephone;
                row["Account"] = item.Account ? "Active" : "Inactive";
                row["Balance"] = item.AccountBalance;
                row["Date"] = DateTime.Now;

                table.Rows.Add(row);
            }

            return table;
        }
        public override ClientData CreateGenericFromDataTable(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                throw new ArgumentException("The provided DataTable is null or empty.");
            var row = table.Rows[0];
            var ClientData = new ClientData
            {
                IdClient = Convert.ToInt32(row["IdClient"]),
                Document = row["Document"].ToString(),
                FullName = row["FullName"].ToString(),
                Email = row["Email"].ToString(),
                Telephone = row["Tel"].ToString(),
                Account = row["Account"].ToString() == "Active",
                AccountBalance = Convert.ToDecimal(row["Balance"]),
                CreationDate = DateTime.Now,
            };

            return ClientData;
        }

        public override void Search(string type, string filter)
        {
            List<ClientData> list = new List<ClientData>();
            list = Items.ToList();
            if(filter != "")
            {
                foreach (var item in Items.ToList())
                {
                    switch (type)
                    {
                        case "IdClient": if (!item.IdClient.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "ID": if (!item.Document.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "FullName": if (!item.FullName.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Email": if (!item.Email.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Tel": if (!item.Telephone.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
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
