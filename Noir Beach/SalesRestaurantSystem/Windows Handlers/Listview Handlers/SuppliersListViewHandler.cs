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
    internal class SuppliersListViewHandler : ListViewPanelController<SupplierData>
    {
        public SuppliersListViewHandler(Window win) : base(win){}

        public override void RemoveItemToList(SupplierData value)
        {
            var element = Items.First(x => x.IdSupplier == value.IdSupplier);
            Items.Remove(element);
        }
        public override DataTable CreateDataTableFromGeneric(List<SupplierData> items)
        {
            var table = new DataTable();

            table.Columns.Add("IdSupplier", typeof(int));
            table.Columns.Add("Document", typeof(string));
            table.Columns.Add("FiscalName", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Tel", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            foreach (var item in items)
            {
                var row = table.NewRow();
                row["IdSupplier"] = item.IdSupplier;
                row["Document"] = item.Document;
                row["FiscalName"] = item.FiscalName;
                row["Email"] = item.Email;
                row["Tel"] = item.Telephone;
                row["Date"] = DateTime.Now;

                table.Rows.Add(row);
            }

            return table;
        }
        public override SupplierData CreateGenericFromDataTable(DataTable table)
        {
            if (table == null || table.Rows.Count == 0)
                throw new ArgumentException("The provided DataTable is null or empty.");
            var row = table.Rows[0];
            var SupplierData = new SupplierData
            {
                IdSupplier = Convert.ToInt32(row["IdSupplier"]),
                Document = row["Document"].ToString(),
                FiscalName = row["FiscalName"].ToString(),
                Email = row["Email"].ToString(),
                Telephone = row["Tel"].ToString(),
                CreationDate = DateTime.Now,
            };

            return SupplierData;
        }

        public override void Search(string type, string filter)
        {
            List<SupplierData> list = new List<SupplierData>();
            list = Items.ToList();
            if(filter != "")
            {
                foreach (var item in Items.ToList())
                {
                    switch (type)
                    {
                        case "Account": if (!item.IdSupplier.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "ID": if (!item.Document.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Full Name": if (!item.FiscalName.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Email": if (!item.Email.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
                        case "Telephone": if (!item.Telephone.ToString().ToLower().Replace(" ", "").Contains(filter.ToLower().Replace(" ", ""))) list.Remove(item); break;
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
